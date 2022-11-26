using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
//using MySqlConnector;
using WebDB.Models;
using WebStudents.Models;

namespace WebDB.Controllers
{
    public class StudentsController : Controller
    {

        static List<Student> students = new List<Student>();
        //static MySqlConnectionStringBuilder constring = new MySqlConnectionStringBuilder()
        //{
        //    Server = "172.25.0.3",
        //    Port = 3306,
        //    UserID = "root",
        //    Password = "oracle",
        //    Database = "students"
        //};
        public static string Constring;
        public StudentsController()
        {
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Index(string studentsGroups, string searchString)
        {
            try
            {
                ViewData["errors"] = "";
                students.Clear();
                using var connection = new MySqlConnection(Constring.ToString());
                connection.Open();

                using var command = new MySqlCommand("SELECT * FROM students", connection);
                var reader = command.ExecuteReader();
                var result = new List<Student>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var student = new Student();
                        student.Id = reader["Id"].ToString();
                        student.FirstName = reader["first_name"].ToString();
                        student.LastName = reader["last_name"].ToString();
                        student.Group = reader["groupe"].ToString();
                        result.Add(student);
                        students.Add(student);
                    }
                }
                connection.Close();

                if (!string.IsNullOrEmpty(studentsGroups))
                {
                    result = result.Where(student => student.Group == studentsGroups).ToList();
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    result = result.Where(student => student.LastName == searchString).ToList();
                }

                var re = new StudentsView()
                {
                    Students = result,
                    Groups = new SelectList(result.Select(x => x.Group).Distinct())
                };
                return View(re);
            }
            catch (Exception e)
            {
                ViewData["errors"] = e.Message.Replace("'", "");
                return View(new ErrorViewModel());
            }
        }

        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Group")] Student student)
        {

            ViewData["errors"] = "";
            if (ModelState.IsValid)
            {
                try
                {
                    using var connection = new MySqlConnection(Constring.ToString());
                    connection.Open();
                    MySqlParameter parameter = new MySqlParameter();

                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO students VALUES(@Id,@FirstName,@LastName,@Group)";

                    command.Parameters.AddWithValue("@Id", student.Id);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Group", student.Group);

                    command.ExecuteNonQuery();
                    connection.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ViewData["errors"] = e.Message.Replace("'", "");

                    return View(student);
                }
            }
            return View(student);

        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = students.FirstOrDefault(x => x.Id == id);
            if (candidate == null) return NotFound();
            return View(candidate);
        }

        //POST: Movies/Delete/2
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                ViewData["errors"] = "";
                using var connection = new MySqlConnection(Constring.ToString());
                connection.Open();
                using var command = new MySqlCommand("Delete from students where id=" + id, connection);
                command.ExecuteNonQuery();
                connection.Close();


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewData["errors"] = e.Message.Replace("'", "");

                return RedirectToAction(nameof(Index));
            }
        }

    }
}

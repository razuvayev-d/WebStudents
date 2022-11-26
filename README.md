# WebStudents
## WebStudents
ASP.NET приложение, позволяет создавать/удалять студентов и искать их по группе и по фамилии в базе данных students.

## DB 
Скрипты и докерфайл для создания базы данных students
Создание образа:
`docker build -t mysqldb -f Dockerfile .`

## Compose
docker-compose.yml файл для согласованного запуска контейнеров данного приложения, базы данных и adminer.

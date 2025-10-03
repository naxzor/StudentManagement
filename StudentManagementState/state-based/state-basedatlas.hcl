env "local" {
  url     = "postgres://postgres:1234@host.docker.internal:5432/StudentManagement_State?sslmode=disable"
  dev_url = "postgres://postgres:1234@host.docker.internal:5432/dev?sslmode=disable"
}

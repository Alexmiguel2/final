using System;
using System.Data.SqlClient;

namespace EscuelaPOO
{
    // Clase base abstracta
    abstract class Persona
    {
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public Persona(string nombre)
        {
            Nombre = nombre;
        }

        public virtual void Saludar()
        {
            Console.WriteLine($"Hola, soy {Nombre}.");
        }
    }

    // Clase Estudiante
    class Estudiante : Persona
    {
        public string Carrera { get; set; }

        public Estudiante(string nombre, string carrera) : base(nombre)
        {
            Carrera = carrera;
        }

        public override void Saludar()
        {
            Console.WriteLine($"Hola, soy {Nombre} y estudio {Carrera}.");
        }

        public void GuardarEnBD()
        {
            string conexion = "Server=P-ET-DTIC-01\\SQLEXPRESS;Database=EscuelaDB;Trusted_Connection=True;Connect Timeout=30;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();
                string query = "INSERT INTO Estudiantes (Nombre, Carrera) VALUES (@Nombre, @Carrera)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    cmd.Parameters.AddWithValue("@Carrera", Carrera);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Estudiante guardado en la base de datos.\n");
        }

        public static void MostrarTodos()
        {
            string conexion = "Server=P-ET-DTIC-01\\SQLEXPRESS;Database=EscuelaDB;Trusted_Connection=True;Connect Timeout=30;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();
                string query = "SELECT Id, Nombre, Carrera FROM Estudiantes";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n Lista de estudiantes:");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        string carrera = reader.GetString(2);
                        Console.WriteLine($"- ID: {id} | Nombre: {nombre} | Carrera: {carrera}");
                    }
                }
            }
            Console.WriteLine();
        }
    }

    // Clase principal con menú
    class Program
    {
        static void Main(string[] args)
        {
            int opcion = 0;
            do
            {
                Console.WriteLine("===== MENÚ ESCUELA =====");
                Console.WriteLine("1. Ingresar estudiante");
                Console.WriteLine("2. Mostrar todos los estudiantes");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                string entrada = Console.ReadLine();

                if (!int.TryParse(entrada, out opcion))
                {
                    Console.WriteLine("Entrada inválida. Intente de nuevo.\n");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        Console.Write("\nIngrese el nombre del estudiante: ");
                        string nombre = Console.ReadLine();

                        Console.Write("Ingrese la carrera del estudiante: ");
                        string carrera = Console.ReadLine();

                        Estudiante estudiante = new Estudiante(nombre, carrera);
                        estudiante.GuardarEnBD();
                        break;

                    case 2:
                        Estudiante.MostrarTodos();
                        break;

                    case 0:
                        Console.WriteLine("Saliendo del programa...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }

            } while (opcion != 0);
        }
    }
}

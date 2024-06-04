using System;
using System.Collections.Generic;

class Calculadora
{
    // Clase que representa una operación
    class Operacion
    {
        public string Tipo { get; set; }
        public double Numero1 { get; set; }
        public double Numero2 { get; set; }
        public string Resultado { get; set; }

        // Constructor para inicializar una operación
        public Operacion(string tipo, double numero1, double numero2, string resultado)
        {
            Tipo = tipo;
            Numero1 = numero1;
            Numero2 = numero2;
            Resultado = resultado;
        }

        // Método para representar la operación como una cadena de texto
        public override string ToString()
        {
            return $"{Tipo}: {Numero1} y {Numero2} = {Resultado}";
        }
    }

    static void Main(string[] args)
    {
        try
        {
            // Preguntar al usuario si desea trabajar con números enteros o decimales
            Console.WriteLine("¿Desea realizar operaciones con números enteros (1) o decimales (2)?");
            if (!int.TryParse(Console.ReadLine(), out int tipoNumero) || (tipoNumero != 1 && tipoNumero != 2))
            {
                throw new ArgumentException("Debe ingresar 1 para enteros o 2 para decimales.");
            }

            // Diccionario de operaciones utilizando expresiones lambda
            var operaciones = new Dictionary<int, Func<double, double, double>>()
            {
                { 1, (num1, num2) => num1 + num2 },
                { 2, (num1, num2) => num1 - num2 },
                { 3, (num1, num2) => num1 * num2 },
                { 4, (num1, num2) => num2 != 0 ? num1 / num2 : double.NaN }
            };

            // Lista para almacenar el registro de operaciones
            var registroOperaciones = new List<Operacion>();

            // Bucle principal del programa
            while (true)
            {
                // Limpiar la pantalla
                Console.Clear();

                // Mostrar el menú de operaciones
                Console.WriteLine("..........-CALCULADORA- -•*.•.......");
                Console.WriteLine("\nMenú de operaciones:");
                Console.WriteLine("1. Sumar");
                Console.WriteLine("2. Restar");
                Console.WriteLine("3. Multiplicar");
                Console.WriteLine("4. Dividir");
                Console.WriteLine("5. Ver registro de operaciones");
                Console.WriteLine("0. Salir");

                // Leer la opción seleccionada por el usuario
                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    continue;
                }

                if (opcion == 0) break;

                // Limpiar la pantalla nuevamente
                Console.Clear();

                if (opcion == 5)
                {
                    // Mostrar el registro de operaciones
                    Console.WriteLine("Registro de operaciones:\n");
                    if (registroOperaciones.Count == 0)
                    {
                        Console.WriteLine("No hay operaciones registradas.");
                    }
                    else
                    {
                        foreach (var registro in registroOperaciones)
                        {
                            Console.WriteLine(registro);
                        }
                    }
                }
                else if (operaciones.ContainsKey(opcion))
                {
                    // Pedir los números al usuario
                    double num1 = double.NaN, num2 = double.NaN;
                    bool num1Valid = PedirNumero("Ingrese el primer número:", out num1);
                    bool num2Valid = PedirNumero("Ingrese el segundo número:", out num2);

                    if (!num1Valid)
                    {
                        Console.WriteLine("Entrada inválida para el primer número. Intente de nuevo.");
                        registroOperaciones.Add(new Operacion(GetOperacionNombre(opcion), num1, num2, "Error (primer número inválido)"));
                        continue;
                    }

                    if (!num2Valid)
                    {
                        Console.WriteLine("Entrada inválida para el segundo número. Intente de nuevo.");
                        registroOperaciones.Add(new Operacion(GetOperacionNombre(opcion), num1, num2, "Error (segundo número inválido)"));
                        continue;
                    }

                    // Realizar la operación seleccionada
                    double resultado = operaciones[opcion](num1, num2);
                    string resultadoStr = (tipoNumero == 1 ? ((int)resultado).ToString() : resultado.ToString());

                    // Verificar si el resultado es un error (división por cero)
                    if (double.IsNaN(resultado))
                    {
                        Console.WriteLine("¡Error! No se puede dividir por cero.");
                        registroOperaciones.Add(new Operacion(GetOperacionNombre(opcion), num1, num2, "Error (división por cero)"));
                    }
                    else
                    {
                        Console.WriteLine("El resultado es: " + resultadoStr);
                        registroOperaciones.Add(new Operacion(GetOperacionNombre(opcion), num1, num2, resultadoStr));
                    }
                }
                else
                {
                    // Manejar opción no válida
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    registroOperaciones.Add(new Operacion("Error", double.NaN, double.NaN, "Opción no válida"));
                }

                // Esperar a que el usuario presione una tecla antes de continuar
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones y mostrar el mensaje de error
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Método para pedir un número al usuario
    static bool PedirNumero(string mensaje, out double numero)
    {
        Console.WriteLine(mensaje);
        return double.TryParse(Console.ReadLine(), out numero);
    }

    // Método para obtener el nombre de la operación basado en la opción seleccionada
    static string GetOperacionNombre(int opcion)
    {
        return opcion switch
        {
            1 => "Suma",
            2 => "Resta",
            3 => "Multiplicación",
            4 => "División",
            _ => "Operación desconocida"
        };
    }
}

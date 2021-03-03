using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppMutex_L
{

  
    class Program
    {
        static void Main(string[] args)
        {
            int menu = 0;
            Mutex mutex = new Mutex();
            do
            {
                Console.WriteLine("1-Task 1");
                Console.WriteLine("2-Task 2");
                Console.WriteLine("0-EXIT");
                Console.Write(">>");
                Int32.TryParse(Console.ReadLine(), out menu);

                switch (menu)
                {
                    //                    Задание 1
                    //Создайте приложение, использующее механизм мьютексов. Создайте в коде
                    //приложения несколько потоков. Первый поток отображает числа от 0 до 20 в
                    //порядке возрастания.Второй поток ожидает, когда завершится первый, после чего
                    //отображает числа от 0 до 10 в обратном порядке.Вывод данных необходимо
                    //выполнять в консоль.
                    case 1:
                        {
                            mutex.WaitOne();
                            Console.WriteLine("Первый поток");
                            Thread myThread = new Thread(new ThreadStart(ShowNumbersFrom_0_To_20));
                          
                            myThread.Start();
                            
                            Console.WriteLine("Второй поток");
                            mutex.ReleaseMutex();
                            mutex.WaitOne();
                            Thread thread2 = new Thread(new ThreadStart(ShowNumbersFrom_10_To_0));
                            thread2.Start();
                            mutex.ReleaseMutex();
                            Console.ReadKey();
                        }
                        break;
//                        Задание 2
//Создайте приложение, использующее механизм мьютексов. Создайте в коде
//приложения несколько потоков. Каждый из потоков получает массив с данными.
//Первый поток должен модифицировать элементы массивы, увеличив каждый на
//некоторое случайное число(элемент массива + случайное число).Второй поток
//ожидает, когда пройдет модификация всего массива, после чего находит
//максимальное значение в этом массиве и отображает его на экран. Вывод данных
//(массив и максимум) необходимо выполнять в консоль.
                    case 2:
                        {
                            mutex.WaitOne();
                            Task<List<int>> task1 = new Task<List<int>>(() => FillArrWithMutex());
                            task1.Start();
                            mutex.ReleaseMutex();
                            task1.Wait();
                            Console.WriteLine("Максимальное значение");
                            Task task2 = new Task(() => ShowMax(task1.Result));
                            task2.Start();
                        }
                        break;

                }

            } while (menu != 0);
        }


        /// <summary>
        /// Метод отображает числа от 0 до 20
        /// </summary>
        static void ShowNumbersFrom_0_To_20()
        {

            Mutex myMutex = new Mutex();
            myMutex.WaitOne();
            for (int i = 0; i <= 20; i++)
            {
                Console.Write($"{i} |");
            }
            Console.WriteLine();
            myMutex.ReleaseMutex();
        }

        /// <summary>
        /// Метод отображает числа от 10 до 0
        /// </summary>
        static void ShowNumbersFrom_10_To_0()
        {
          
            for (int i = 10; i >= 0; i--)
            {
                Console.Write($"{i} |");
            }

            Console.WriteLine();
        
        }


        /// <summary>
        /// метод увеличивает число на случайное значение
        /// </summary>
        /// <param name="number"></param>
        static int RandPlusValue(int number)
        {
            Random random = new Random();
            number += random.Next(0, 100);
            return number;
        }



         static List<int> FillArrWithMutex()
        {
            Mutex mutex = new Mutex(true, "MyMutex");
            Mutex.OpenExisting("MyMutex");
            List<int> arr = new List<int>();
          
            for (int i = 0; i < 10; i++)
            {
                int value = 0;
                mutex.WaitOne();
                value = RandPlusValue(i);
                arr.Add(value);
                mutex.ReleaseMutex();
            }
            Console.WriteLine("Значения массива");
            for (int i = 0; i < arr.Count; i++)
            {
                Console.Write($"{arr[i]} |");
            }
            
            return arr;
        }

        static void ShowMax(List<int> arr)
        {
           
          
            Console.WriteLine($"{arr.Max()}");
        }

    }

   
}

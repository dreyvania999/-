namespace Метод_потенциалов
{
    internal class Program
    {
        static int[,] matr;
        static int[] mPost;
        static int[] zPotreb;


        static void Main()
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("1 - Ввести данные вручную.");
                Console.WriteLine("2 - Заполнить данные автоматически (по условию задачи).");
                Console.WriteLine("3 - Очистить экран.");
                Console.WriteLine("4 - Завершить работу.");
                Console.Write("Выберите действие: ");
                if (int.TryParse(Console.ReadLine(), out int result) && result > 0 && result < 5)
                {//выбор варианта работы программы
                    switch (result)
                    {
                        case 1:
                            Console.Clear();
                            HandVvod();
                            Reshenie();
                            break;
                        case 2:
                            Console.Clear();
                            PoYmolchaniu();
                            Reshenie();
                            break;
                        case 3:
                            Console.Clear();
                            break;
                        case 4:
                            check = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Выберите один из предложенных вариантов!");
                }
            }
        }

        /// <summary>
        /// Определение вырожденности
        /// </summary>
        /// <param name="divisionArray">Распределение</param>
        /// <param name="m">Поставщики</param>
        /// <param name="n">Потребители</param>
        static void Virozdin(bool[,] divisionArray, int m, int n)
        {
            int kol = 0;
            foreach (bool item in divisionArray)
            {
                if (item)// подсчет количества элементов в массиве
                {
                    kol++;
                }
            }
            if (m + n - 1 != kol)
            {
                int min = 16;
                int iMin = 0;
                int jMin = 0;
                for (int i = 0; i < mPost.Length; i++)
                {
                    for (int j = 0; j < zPotreb.Length; j++)
                    {
                        if (!divisionArray[i, j])
                        {
                            if (matr[i, j] < min) //if (matr[i, j] <= min)
                            {
                                min = matr[i, j];
                                iMin = i;
                                jMin = j;
                            }
                        }
                    }
                }
                divisionArray[iMin, jMin] = true;
            }
        }

        /// <summary>
        /// Определение ячеек для цикла перераспределения
        /// </summary>
        /// <param name="divisionArrayBool">Информация о распределении</param>
        /// <param name="iMaxDelt">Индекс i максимальной дельты</param>
        /// <param name="jMaxDelt">Индекс j максимальной дельты</param>
        /// <returns>Строковый массив с индексами ячеек для цикла перераспределения</returns>
        static string[] Cycle(bool[,] divisionArrayBool, int iMaxDelt, int jMaxDelt)
        {
            Console.WriteLine("Введите индексы ячеек, через которые будет проходить цикл, без разделителей.\nНапример: 13 - ячейка [1;3].");
            Console.WriteLine("Цикл можно проводить только через ячейки, в которых находятся единицы.");
            Console.WriteLine("Для завершения ввода введите 0.");
            Console.WriteLine("Цикл начинается из ячейки [{0},{1}]. Введите следующие ячейки.", iMaxDelt + 1, jMaxDelt + 1);
            string[] array = new string[1];
            array[0] = Convert.ToString(iMaxDelt) + Convert.ToString(jMaxDelt);
            int i = 1;
            string check;
            while (true)
            {
                while (true)
                {
                    if (i % 2 == 0)//выбор знакаа перед х
                    {
                        Console.Write("Введите ячейку цикла (+х): ");
                    }
                    else
                    {
                        Console.Write("Введите ячейку цикла (-х): ");
                    }

                    check = Console.ReadLine();
                    if (int.TryParse(check, out int result) && result > 10 && result < 35 || check == "0")// проверка правильности выбора ячейки пользователем
                    {
                        if (check == "0")
                        {
                            Console.Clear();
                            return array;
                        }
                        if (divisionArrayBool[Convert.ToInt32(check.Substring(0, 1)) - 1, Convert.ToInt32(check.Substring(1, 1)) - 1])
                        {
                            Array.Resize(ref array, array.Length + 1);
                            array[i] = check;
                            i++;
                            break;
                        }
                    }
                    Console.WriteLine("Ошибка. Введите ячейку корректно!");
                }
            }
        }

        /// <summary>
        /// Расчёт потенциалов, дельты и цикла перераспределения (2, 3, 4 шаги)
        /// </summary>
        /// <param name="divisionArray">Распределение</param>
        /// <param name="divisionArrayBool">Информация о распределении (есть ли связь между поставщиком и потребителем)</param>
        /// <returns>Оптимальное распределение (true) или нет (false)</returns>
        static bool calcPotensial(int[,] divisionArray, bool[,] divisionArrayBool)
        {
            int[] v = new int[mPost.Length];
            int[] u = new int[zPotreb.Length];
            bool[] vbool = new bool[mPost.Length];
            bool[] ubool = new bool[zPotreb.Length];
            ubool[0] = true;

            for (int i = 0; i < mPost.Length; i++)
            {
                if (divisionArrayBool[i, 0])
                {
                    v[i] = matr[i, 0];
                    vbool[i] = true;
                }
            }

            bool checkV = true;
            bool checkU = true;
            while (checkV || checkU)
            {
                for (int i = 0; i < mPost.Length; i++)
                {
                    for (int j = 1; j < zPotreb.Length; j++)
                    {
                        if (divisionArrayBool[i, j])
                        {
                            if (ubool[j])
                            {
                                v[i] = matr[i, j] - u[j];
                                vbool[i] = true;
                            }

                            if (vbool[i])
                            {
                                u[j] = matr[i, j] - v[i];
                                ubool[j] = true;
                            }
                        }
                    }
                }

                checkU = false;
                foreach (bool item in ubool)
                {
                    if (item == false)
                    {
                        checkU = true;
                        break;
                    }
                }
                checkV = false;
                foreach (bool item in vbool)
                {
                    if (item == false)
                    {
                        checkV = true;
                        break;
                    }
                }
            }

            int[,] delta = new int[mPost.Length, zPotreb.Length];
            for (int i = 0; i < mPost.Length; i++)
            {
                for (int j = 0; j < zPotreb.Length; j++)
                {
                    if (!divisionArrayBool[i, j])
                    {
                        delta[i, j] = v[i] + u[j] - matr[i, j];
                    }
                }
            }

            bool check = false;
            foreach (int item in delta)
            {
                if (item > 0)
                {
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                return true;
            }

            int max = delta[0, 0]; // просчет дельта(и поск максимальных значений)
            int iMax = 0;
            int jMax = 0;
            for (int i = 0; i < mPost.Length; i++)
            {
                for (int j = 0; j < zPotreb.Length; j++)
                {
                    if (delta[i, j] > max)
                    {
                        max = delta[i, j];
                        iMax = i;
                        jMax = j;
                    }
                }
            }

            for (int i = 0; i < mPost.Length; i++)
            {
                for (int j = 0; j < zPotreb.Length; j++)
                {
                    if (divisionArrayBool[i, j])
                    {
                        Console.Write(1 + "\t");
                    }
                    else
                    {
                        Console.Write(0 + "\t");
                    }
                }
                Console.WriteLine();
            }

            string[] cycleArray = Cycle(divisionArrayBool, iMax, jMax);

            int iMin = Convert.ToInt32(cycleArray[1].Substring(0, 1)) - 1;
            int jMin = Convert.ToInt32(cycleArray[1].Substring(1, 1)) - 1;
            int min = divisionArray[iMin, jMin];
            for (int i = 3; i < cycleArray.Length; i += 2)
            {
                iMin = Convert.ToInt32(cycleArray[i].Substring(0, 1)) - 1;
                jMin = Convert.ToInt32(cycleArray[i].Substring(1, 1)) - 1;
                if (divisionArray[iMin, jMin] < min)
                {
                    min = divisionArray[iMin, jMin];
                }
            }

            int indexI = Convert.ToInt32(cycleArray[0].Substring(0, 1));
            int jndexJ = Convert.ToInt32(cycleArray[0].Substring(1, 1));
            divisionArray[indexI, jndexJ] += min;
            if (divisionArray[indexI, jndexJ] > 0)
            {
                divisionArrayBool[indexI, jndexJ] = true;
            }

            for (int c = 1; c < cycleArray.Length; c++)
            {
                int i = Convert.ToInt32(cycleArray[c].Substring(0, 1)) - 1;
                int j = Convert.ToInt32(cycleArray[c].Substring(1, 1)) - 1;
                if (c % 2 == 0)
                {
                    divisionArray[i, j] += min;
                    if (divisionArray[i, j] > 0)
                    {
                        divisionArrayBool[i, j] = true;
                    }
                }
                else
                {
                    divisionArray[i, j] -= min;
                    if (divisionArray[i, j] == 0)
                    {
                        divisionArrayBool[i, j] = false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Расчёт функции и заключения договоров
        /// </summary>
        static void Reshenie()
        {
            int[] m = new int[mPost.Length];
            int[] n = new int[zPotreb.Length];
            bool[,] divisionArrayBool = new bool[m.Length, n.Length];
            int[,] divisionArray = new int[m.Length, n.Length];
            int[,] firstDivision = new int[m.Length, n.Length];
            bool check = false;
            int func = 0;
            string funcS = "";

            Array.Copy(matr, firstDivision, matr.Length);
            Array.Copy(mPost, m, mPost.Length);
            Array.Copy(zPotreb, n, zPotreb.Length);

            int sum = 1;

            while (sum != 0)
            {
                int min = 16;
                int iMin = 0;
                int jMin = 0;

                for (int i = 0; i < m.Length; i++)
                {
                    for (int j = 0; j < n.Length; j++)
                    {
                        if (firstDivision[i, j] < min && firstDivision[i, j] > 0)
                        {
                            min = firstDivision[i, j];
                            iMin = i;
                            jMin = j;
                        }
                    }
                }

                firstDivision[iMin, jMin] = 0;

                if (m[iMin] > 0 && n[jMin] > 0)
                {
                    if (n[jMin] < m[iMin])
                    {
                        func += matr[iMin, jMin] * n[jMin];
                        funcS += matr[iMin, jMin] + "*" + n[jMin] + " + ";
                        divisionArray[iMin, jMin] = n[jMin];
                        divisionArrayBool[iMin, jMin] = true;
                        m[iMin] -= n[jMin];
                        n[jMin] = 0;
                    }
                    else
                    {
                        func += matr[iMin, jMin] * m[iMin];
                        funcS += matr[iMin, jMin] + "*" + m[iMin] + " + ";
                        divisionArray[iMin, jMin] = m[iMin];
                        divisionArrayBool[iMin, jMin] = true;
                        n[jMin] -= m[iMin];
                        m[iMin] = 0;
                    }
                }

                sum = 0;
                foreach (int item in m)
                {
                    sum += item;
                }
            }

            Virozdin(divisionArrayBool, m.Length, n.Length);

            funcS = "Ответ: Fопт = " + funcS.Substring(0, funcS.Length - 3) + " = " + func + " у.д.е.";

            check = calcPotensial(divisionArray, divisionArrayBool);

            if (!check)
            {
                while (check != true)
                {
                    funcS = "";
                    func = 0;

                    Array.Copy(mPost, m, mPost.Length);
                    Array.Copy(zPotreb, n, zPotreb.Length);

                    for (int i = 0; i < m.Length; i++)
                    {
                        for (int j = 0; j < n.Length; j++)
                        {
                            if (divisionArray[i, j] > 0)
                            {
                                if (n[j] < m[i])
                                {
                                    func += matr[i, j] * divisionArray[i, j];
                                    funcS += matr[i, j] + "*" + divisionArray[i, j] + " + ";
                                }
                                else
                                {
                                    func += matr[i, j] * divisionArray[i, j];
                                    funcS += matr[i, j] + "*" + divisionArray[i, j] + " + ";
                                }
                            }
                        }
                    }

                    funcS = "Ответ: Fопт = " + funcS.Substring(0, funcS.Length - 3) + " = " + func + " у.д.е.";

                    check = calcPotensial(divisionArray, divisionArrayBool);
                }
            }
            Console.WriteLine(funcS + "\n");

            //Console.WriteLine("\t\tЗаключение договоров");
            //for (int i = 0; i < m.Length; i++)
            //{
            //    for (int j = 0; j < n.Length; j++)
            //    {
            //        if (divisionArray[i, j] > 0)
            //        {
            //            Console.WriteLine("{0}-й поставщик с {1}-м потребителем на {2} ед. продукции", i + 1, j + 1, divisionArray[i, j]);
            //        }
            //    }
            //}
            Console.WriteLine();
        }

        /// <summary>
        /// Заполнение исходных данных по условию задачи
        /// </summary>
        static void PoYmolchaniu()
        {
            matr = new int[3, 4]
            {
                {9,5,13,10},
                {6,13,8,12},
                {13,8,14,7}
            };

            mPost = new int[3]
            {25,50,25};

            zPotreb = new int[4]
            { 40,10,20,30};
        }

        /// <summary>
        /// Ввод исходных данных вручную
        /// </summary>
        static void HandVvod()
        {
            int m, n;
            while (true)
            {
                Console.Write("Введите количество поставщиков: ");
                if (int.TryParse(Console.ReadLine(), out int result) && result > 1 && result < 11)
                {
                    m = result;
                    break;
                }
                Console.WriteLine("Ошибка. Введите значение корректно!");
            }

            while (true)
            {
                Console.Write("Введите количество потребителей: ");
                if (int.TryParse(Console.ReadLine(), out int result) && result > 1 && result < 11)
                {
                    n = result;
                    break;
                }
                Console.WriteLine("Ошибка. Введите значение корректно!");
            }

            matr = new int[m, n];
            mPost = new int[m];
            zPotreb = new int[n];

            Console.WriteLine("Заполнение матрицы затрат на перевозку.");
            for (int i = 0; i < mPost.Length; i++)
            {
                for (int j = 0; j < zPotreb.Length; j++)
                {
                    while (true)
                    {
                        Console.Write("Введите [{0},{1}] элемент матрицы: ", i + 1, j + 1);
                        if (int.TryParse(Console.ReadLine(), out int result) && result > 0 && result < 16)
                        {
                            matr[i, j] = result;
                            break;
                        }
                        Console.WriteLine("Ошибка. Введите значение корректно!");
                    }
                }
            }
            while (true)
            {
                for (int i = 0; i < mPost.Length; i++)
                {
                    while (true)
                    {
                        Console.Write("Введите мощность {0}-го поставщика: ", i + 1);
                        if (int.TryParse(Console.ReadLine(), out int result) && result > 1 && result < 151)
                        {
                            mPost[i] = result;
                            break;
                        }
                        Console.WriteLine("Ошибка. Введите значение корректно!");
                    }
                }

                for (int i = 0; i < zPotreb.Length; i++)
                {
                    while (true)
                    {
                        Console.Write("Введите спрос {0}-го потребителя: ", i + 1);
                        if (int.TryParse(Console.ReadLine(), out int result) && result > 1 && result < 151)
                        {
                            zPotreb[i] = result;
                            break;
                        }
                        Console.WriteLine("Ошибка. Введите значение корректно!");
                    }
                }

                Console.Clear();

                int sumM = 0;
                foreach (int item in mPost)
                {
                    sumM += item;
                }
                int sumN = 0;
                foreach (int item in zPotreb)
                {
                    sumN += item;
                }

                if (sumM == sumN)
                {
                    break;
                }
                Console.WriteLine("Ошибка. Суммарные мощности поставщиков и спросов потребителей не равны!");
            }
        }


    }
}
//Строки сортируются по длине;
//В случае некорректных входящих параметров программа сообщит об этом и завершится;
//В случае некорректных данных в файлах программа сообщит "Incorrect parameters", проигнорирует параметр и пойдет дальше, в конечном файле будут только корректные данные.

using System;
using System.Collections.Generic;
using System.IO;

namespace Shift
{
    class Program
    {
        static void Main(string[] args)
        {
            int argsLength = args.Length;
            string sorting = string.Empty;
            string typeOfData = string.Empty;
            string outputFile = string.Empty;
            List<string> inputFile = new List<string>();
            int i = 0;

            if (argsLength > 2)
            {
                if (args[i] == "-a" || args[i] == "-d")
                {
                    sorting = args[0];
                }

                if (args[i] == "-s" || args[i] == "-i")
                {
                    typeOfData = args[i];
                    sorting = "-a";
                    i = 1;
                }
                else if (args[i + 1] == "-s" || args[i + 1] == "-i")
                {
                    typeOfData = args[i + 1];
                    i = 2;
                }
                else
                {
                    Console.WriteLine("Incorrect parameters");
                    return;
                }

                if (args[i].EndsWith(".txt"))
                {
                    outputFile = args[i];
                    i++;
                }
                else
                {
                    Console.WriteLine("Incorrect parameters");
                    return;
                }

                if (i + 1 >= argsLength)
                {
                    Console.WriteLine("Incorrect parameters");
                    return;
                }

                while (i < argsLength)
                {
                    if (args[i].EndsWith(".txt"))
                    {
                        inputFile.Add(args[i]);
                    }
                    else
                    {
                        Console.WriteLine("Incorrect name of file:", args[i]);
                    }

                    i++;
                }

                if (inputFile.Count == 0)
                {
                    Console.WriteLine("Incorrect parameters");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Not enough parameters");
                return;
            }

            List<string> dataFromFiles = new List<string>();

            foreach (string fileName in inputFile)
            {
                StreamReader nextFile = new StreamReader(fileName);

                while (!nextFile.EndOfStream)
                {
                    dataFromFiles.Add(nextFile.ReadLine());
                }
                nextFile.Close();
            }

            dataFromFiles = sortMerg(dataFromFiles, typeOfData, sorting);

            StreamWriter endFile = new StreamWriter(outputFile, false);

            foreach (string el in dataFromFiles)
            {
                endFile.WriteLine(el);
            }

            endFile.Close();
        }

        static List<string> sortMerg(List<string> dataFromFiles, string typeOfData, string sorting)
        {
            int lenList = dataFromFiles.Count;

            if (lenList == 2)
            {
                if (typeOfData == "-s" &&
                    ((dataFromFiles[0].Length > dataFromFiles[1].Length && sorting == "-a") || (dataFromFiles[0].Length < dataFromFiles[1].Length && sorting == "-d")))
                {
                    string temp = dataFromFiles[0];
                    dataFromFiles[0] = dataFromFiles[1];
                    dataFromFiles[1] = temp;
                }
                else if (typeOfData == "-i")
                {
                    int number1;
                    int number2;

                    bool resPars1 = int.TryParse(dataFromFiles[0], out number1);
                    bool resPars2 = int.TryParse(dataFromFiles[1], out number2);

                    if (!resPars1 && !resPars2)
                    {
                        Console.WriteLine("Incorrect parameters, expected integer");
                        dataFromFiles.Clear();
                    }
                    else if (!resPars1)
                    {
                        Console.WriteLine("Incorrect parameters, expected integer");

                        string temp = dataFromFiles[1];
                        dataFromFiles[1] = dataFromFiles[0];
                        dataFromFiles[0] = temp;

                        dataFromFiles.RemoveAt(1);
                    }
                    else if (!resPars2)
                    {
                        Console.WriteLine("Incorrect parameters, expected integer");
                        dataFromFiles.RemoveAt(1);
                    }
                    else if (number1 > number2 && sorting == "-a")
                    {
                        string temp = dataFromFiles[0];
                        dataFromFiles[0] = dataFromFiles[1];
                        dataFromFiles[1] = temp;
                    }
                    else if (number1 < number2 && sorting == "-d")
                    {
                        string temp = dataFromFiles[0];
                        dataFromFiles[0] = dataFromFiles[1];
                        dataFromFiles[1] = temp;
                    }
                }
            }
            else if (lenList > 2)
            {
                List<string> list1 = new List<string>();
                List<string> list2 = new List<string>();

                int numberList2 = lenList / 2;

                if (lenList % 2 > 0)
                {
                    numberList2++;
                }

                list1 = dataFromFiles.GetRange(0, lenList / 2);
                list2 = dataFromFiles.GetRange(lenList / 2, numberList2);

                list1 = sortMerg(list1, typeOfData, sorting);
                list2 = sortMerg(list2, typeOfData, sorting);

                int lenlist1 = list1.Count;
                int lenlist2 = list2.Count;

                int indList1 = 0;
                int indList2 = 0;

                dataFromFiles.Clear();

                for (int i = 0; i < lenList; i++)
                {
                    if (indList1 == lenlist1 && indList2 == lenlist2)
                    {
                        break;
                    }
                    else if (indList1 == lenlist1)
                    {
                        dataFromFiles.Add(list2[indList2]);
                        indList2++;
                    }
                    else if (indList2 == lenlist2)
                    {
                        dataFromFiles.Add(list1[indList1]);
                        indList1++;
                    }
                    else
                    {
                        int number1;
                        int number2;

                        bool resPars1 = true;
                        bool resPars2 = true;

                        if (typeOfData == "-i")
                        {
                            resPars1 = int.TryParse(list1[indList1], out number1);
                            resPars2 = int.TryParse(list2[indList2], out number2);
                        }
                        else
                        {
                            number1 = list1[indList1].Length;
                            number2 = list2[indList2].Length;
                        }

                        if (!resPars1 && !resPars2)
                        {
                            Console.WriteLine("Incorrect parameters, expected integer");
                            indList1++;
                            indList2++;
                        }
                        else if (!resPars1)
                        {
                            Console.WriteLine("Incorrect parameters, expected integer");
                            indList1++;
                        }
                        else if (!resPars2)
                        {
                            Console.WriteLine("Incorrect parameters, expected integer");
                            indList2++;
                        }
                        else if (number1 > number2)
                        {
                            if (sorting == "-a")
                            {
                                dataFromFiles.Add(list2[indList2]);
                                indList2++;
                            }
                            else
                            {
                                dataFromFiles.Add(list1[indList1]);
                                indList1++;
                            }
                        }
                        else if (number1 < number2)
                        {
                            if (sorting == "-a")
                            {
                                dataFromFiles.Add(list1[indList1]);
                                indList1++;
                            }
                            else
                            {
                                dataFromFiles.Add(list2[indList2]);
                                indList2++;
                            }
                        }
                        else
                        {
                            dataFromFiles.Add(list1[indList1]);
                            dataFromFiles.Add(list2[indList2]);
                            indList1++;
                            indList2++;
                        }
                    }
                }
            }

            return dataFromFiles;
        }
    }
}

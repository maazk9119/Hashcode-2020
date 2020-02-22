using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Hashcode
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "d_tough_choices.txt";
            //find the books,lib,days
            string[] file = System.IO.File.ReadAllLines(filename);

            // first line
            string[] first_line = file[0].Split(' ');
            int nb = int.Parse(first_line[0]);
            int nl = int.Parse(first_line[1]);
            int nd = int.Parse(first_line[2]);

            //finds the score of the books
            string[] books_scr_string = file[1].Split(' ');
            int[] books_scr = new int[nb];
            for (int i = 0; i < nb; i++)
            {
                books_scr[i] = int.Parse(books_scr_string[i]);
            }

            int[,] libs = new int[nl, 3];
            List<int>[] lib_books = new List<int>[nl];
            //int[,] lib_books = new int[nl, nb];

            int c = 2;
            for (int i = 0; i < nl; i++)
            {
                string[] libinfo = file[c].Split(' ');
                libs[i, 0] = Convert.ToInt32(libinfo[0]);
                libs[i, 1] = Convert.ToInt32(libinfo[1]);
                libs[i, 2] = Convert.ToInt32(libinfo[2]);

                c++;
                lib_books[i] = new List<int>();
                string[] libboosk = file[c].Split(' ');
                for (int k = 0; k < libboosk.Length; k++)
                {
                    int tb = Convert.ToInt32(libboosk[k]);
                    lib_books[i].Add(tb);
                }
                c++;

            }

            float[] lib_value = new float[nl];

            for (int i = 0; i < nl; i++)
            {
                int sum_book_scr = 0;
                foreach (int item in lib_books[i])
                {
                    sum_book_scr += books_scr[item];
                }
                lib_value[i] = (sum_book_scr / (float)libs[i, 0]) * libs[i, 2];
            }

            List<int> counted = new List<int>();
            int rdays = nd;
            int pl = 0;
            int lb = 0;
            List<int>[] listbook = new List<int>[nl];
            List<List<int>> selectedBooks = new List<List<int>>();
            while (rdays > 0 && lb < nl)
            {
                int r = GetMinLocation(libs, lib_value, nl, counted);
                List<int> foundbooks = Getbooks(lib_books[r], libs[r, 2], rdays);
                if(foundbooks.Count() >0)
                {
                    counted.Add(r);
                    rdays = rdays - libs[r, 1];
                    selectedBooks.Add(foundbooks);
                    pl++;
                }
                Console.WriteLine("Processed {0}", lb);
                lb++;
                
            }

            StreamWriter sw = new StreamWriter(filename+".out");

            sw.WriteLine(pl);
            for (int i = 0; i < counted.Count(); i++)
            {
                sw.WriteLine(string.Format("{0} {1}", counted[i], selectedBooks[i].Count()));
                foreach (int item in selectedBooks[i])
                {
                    sw.Write(string.Format("{0} ", item));
                }
                sw.WriteLine();
            }

            sw.Close();




            //Console.ReadLine();



        }

        static List<int> Getbooks(List<int> books, int perday, int rdays)
        {
            List<int> sbooks = new List<int>();
            int c = 0;
            for (int i = 0; i < rdays; i++)
            {
                for (int j = 0; j < perday; j++)
                {
                    if (c < books.Count())
                        sbooks.Add(books[c]);
                    c++;
                }
            }

            return sbooks;
        }

        static int GetMinLocation(int[,] libs, float[] lib_value, int nl, List<int> counted)
        {
            int min = int.MaxValue;
            float max = float.MinValue;
            int l = -1;
            for (int i = 0; i < nl; i++)
            {
                if (counted.Contains(i))
                    continue;

                if (libs[i, 2] == min)
                {
                    if (max < lib_value[i])
                    {
                        max = lib_value[i];
                        l = i;
                    }
                }
                else if (libs[i, 2] < min)
                {
                    min = libs[i, 2];
                    max = lib_value[i];
                    l = i;
                }
            }

            return l;
        }

    }
}

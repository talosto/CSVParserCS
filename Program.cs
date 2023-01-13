using System.Text;

//для работы программы необходим пакет System.Text.Encoding.CodePages
//регистрируем в системе дополнительные кодировки
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

string path = Environment.CurrentDirectory + @".\source\original-table.csv";

//дополнительно, нужно написать код, распознающий кодировку файла и заменить
//второе свойство передаваемое StreamReader (вместо Encoding.GetEncoding(1251))
//полученной кодировкой

int lineCounter = 0;


List<Language> languages = new List<Language>(lineCounter);

using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1251)))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {

        //считываем первую строку, узнаем количество полей для объекта
        //в будущем - формировать поля в класс, исходя из этого количества, переводя
        //в транслит каждое поле
        if (lineCounter == 0)
        {
            string[] firstLine = line.Split(';');

            foreach (string i in firstLine)
            {
                //здесь должен быть код, определяющий названия полей класса и инициализирующий их
            }

        }
        else
        {
            string[] languageInfos = line.Split(';');

            var name = (string)languageInfos[0];
            var year = languageInfos[1].ToString();
            var author = (string)languageInfos[2];
            var oop = (string)languageInfos[3];
            var activeDev = (string)languageInfos[4];
            var lastVersion = (string)languageInfos[5];
            var lastVerDate = (string)languageInfos[6];

            //bool oopBool = oop == "нет" ? oopBool = false : oopBool = true;
            //bool activeDevBool = activeDev == "нет" ? activeDevBool = false : activeDevBool = true;
            bool oopBool = oop == "да";
            bool activeDevBool = activeDev == "да";

            var lastVerDateSplitted = lastVerDate.Split('.');
            int day = Convert.ToInt32(lastVerDateSplitted[0]);
            int month = Convert.ToInt32(lastVerDateSplitted[1]);
            int yearFor = Convert.ToInt32(lastVerDateSplitted[2]);
            DateOnly lastVerDateDate = new DateOnly(yearFor, month, day);

            var language = new Language();
            language.Name = name;
            language.Year = Convert.ToInt32(year);
            language.Author = author;
            language.Oop = oopBool;
            language.ActiveDev = activeDevBool;
            language.LastVersion = lastVersion;
            language.LastVerDate = lastVerDateDate;

            languages.Add(language);

        }

        lineCounter++;
    }
}

Console.WriteLine("Введите год или диапазон годов через дефис:");
var zapros = Console.ReadLine();

int yearBegin = 0;
int yearEnd = 0;
int zaprosNum = 0;

if (zapros.Contains("-"))
{
    var zaprosSplitted = zapros.Split('-');
    yearBegin = Convert.ToInt32(zaprosSplitted[0]);
    yearEnd = Convert.ToInt32(zaprosSplitted[1]);
}
else
{
    zaprosNum = Convert.ToInt32(zapros);
}

foreach (Language i in languages)
{
    if (yearBegin != 0)
    {
        if ((i.Year > yearBegin - 1) && (i.Year < yearEnd + 1))
        {
            Console.WriteLine("В {0}-том году придуман язык {1} \nА его автор {2}\n", i.Year, i.Name, i.Author);
        } 
    }
    else if (zaprosNum != 0)
    {
        if (i.Year == zaprosNum)
        {
            Console.WriteLine("В {0}-том году придуман язык {1} \nА его автор {2}\n", i.Year, i.Name, i.Author);
        }
    }
    else
    {
        if (yearBegin == 0 && zaprosNum == 0)
        {
            Console.WriteLine("Что-то пошло не так...");
        }
    }
}
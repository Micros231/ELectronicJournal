using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Views.Error
{
    public static class ErrorHandler
    {
        public static string GetErrorMessage(int? code)
        {
            var stringBuilder = new StringBuilder();
            if (code.HasValue)
            {
                var statusCode = code.Value;
                switch (statusCode)
                {
                    case 400:
                        stringBuilder.AppendLine("Неверный запрос!");
                        break;
                    case 401:
                        stringBuilder.AppendLine("У вас нет доуступа к этому ресурсу. Вы не авторизованы!");
                        break;
                    case 403:
                        stringBuilder.AppendLine("У вас нет доуступа к этому ресурсу. Запрещено!");
                        break;
                    case 404:
                        stringBuilder.AppendLine("Не найден ресурс!");
                        break;
                    default:
                        stringBuilder.AppendLine("Непредвиденная ошибка, напишите администратору для решения проблемы");
                        break;
                }
            }
            else
            {
                stringBuilder.AppendLine("Непредвиденная ошибка, напишите администратору для решения проблемы");
            }
            return stringBuilder.ToString();
        }

    }
}

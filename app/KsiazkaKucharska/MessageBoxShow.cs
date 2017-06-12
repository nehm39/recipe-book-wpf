using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KsiazkaKucharska
{
    public class MessageBoxShow
    {
        public MessageBoxWynik pokazMessageBox(string tytul, string tekst, MessageBoxTyp typ, MessageBoxIkona icon)
        {
            return (MessageBoxWynik)MessageBox.Show(tekst, tytul, (MessageBoxButton)typ, (MessageBoxImage)icon);
        }

    }

    public enum MessageBoxTyp
    {
        Normalny = MessageBoxButton.OK,
        TakNie = MessageBoxButton.YesNo
    };

    public enum MessageBoxIkona
    {
        Informacyjna = MessageBoxImage.Information,
        Blad = MessageBoxImage.Error,
        Stop = MessageBoxImage.Stop,
        Ostrzezenie = MessageBoxImage.Warning,
        Pytanie = MessageBoxImage.Question
    };

    public enum MessageBoxWynik
    {
        Ok = MessageBoxResult.OK,
        Tak = MessageBoxResult.Yes,
        Nie = MessageBoxResult.No
    };
}

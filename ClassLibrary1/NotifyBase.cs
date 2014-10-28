using System;
using System.ComponentModel;

namespace Diagram
{
    // Abstract base class. Bruges til at samle INotifyPropertyChanged funktionaliteten, så den ikke behøves implementeres i alle de almindelige model klasser.
    // Formålet med INotifyPropertyChanged er at når en attribut for en klasse ændres, så smides en event der giver GUI'en besked om ændringen.
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        // Her defineres den event der skal smides for at GUI'en ved at en attribut er blevet ændret.
        public event PropertyChangedEventHandler PropertyChanged;

        // Denne metode bruges til at smide PropertyChanged eventen og skal kaldes i alle set metoderne, for alle model klassernes attributer.
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

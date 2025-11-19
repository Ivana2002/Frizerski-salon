using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace IvanaDrugi.Resources
{
    public class LocalizationManager : INotifyPropertyChanged
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _resources = new()
        {
            ["sr"] = new Dictionary<string, string>
            {
                {"AppName", "Frizerski Salon"},
                {"LoginTitle", "Prijava - Frizerski Salon"},
                {"SalonName", "Frizerski Salon 'Ljepota'"},
                {"UsernameLabel", "Korisničko ime:"},
                {"PasswordLabel", "Lozinka:"},
                {"LoginButton", "Prijavi se"},
                {"MainMenu_Services", "Usluge"},
                {"MainMenu_Reservations", "Rezervacije"},
                {"MainMenu_Invoices", "Računi"},
                {"MainMenu_Settings", "Postavke"},
                {"Services_Title", "Usluge frizerskog salona"},
                {"Services_View", "Pregled usluga"},
                {"Services_Add", "➕ Dodaj novu uslugu"},
                {"Services_Search", "Pretraga usluga"},
                {"Services_Delete", "Obriši uslugu"},
                {"Services_SearchByTitle", "Pretraži po nazivu:"},
                {"Reservations_Title", "Rezervacije frizerskog salona"},
                {"Reservations_ClientName", "Ime klijenta:"},
                {"Reservations_Phone", "Telefon:"},
                {"Reservations_DateTime", "Datum i vreme:"},
                {"Reservations_Services", "Usluge:"},
                {"Reservations_Create", "Kreiraj rezervaciju"},
                {"Invoices_Title", "Računi frizerskog salona"},
                {"Invoices_Number", "Broj računa:"},
                {"Invoices_Date", "Datum izdavanja:"},
                {"Invoices_Client", "Klijent:"},
                {"Invoices_Services", "Usluge:"},
                {"Invoices_Issue", "Izdaj račun"},
                {"Settings_ManageUsers", "Upravljanje korisnicima"},
                {"Settings_ChangeTheme", "Promjena teme"},
                {"Settings_ChangeLanguage", "Promjena jezika"},
                {"Button_AddUser", "➕ Dodaj korisnika"}
            },
            ["en"] = new Dictionary<string, string>
            {
                {"AppName", "Hair Salon"},
                {"LoginTitle", "Login - Hair Salon"},
                {"SalonName", "Hair Salon 'Beauty'"},
                {"UsernameLabel", "Username:"},
                {"PasswordLabel", "Password:"},
                {"LoginButton", "Log in"},
                {"MainMenu_Services", "Services"},
                {"MainMenu_Reservations", "Reservations"},
                {"MainMenu_Invoices", "Invoices"},
                {"MainMenu_Settings", "Settings"},
                {"Services_Title", "Hair salon services"},
                {"Services_View", "View services"},
                {"Services_Add", "➕ Add new service"},
                {"Services_Search", "Search services"},
                {"Services_Delete", "Delete service"},
                {"Services_SearchByTitle", "Search by name:"},
                {"Reservations_Title", "Hair salon reservations"},
                {"Reservations_ClientName", "Client name:"},
                {"Reservations_Phone", "Phone:"},
                {"Reservations_DateTime", "Date and time:"},
                {"Reservations_Services", "Services:"},
                {"Reservations_Create", "Create reservation"},
                {"Invoices_Title", "Hair salon invoices"},
                {"Invoices_Number", "Invoice number:"},
                {"Invoices_Date", "Issue date:"},
                {"Invoices_Client", "Client:"},
                {"Invoices_Services", "Services:"},
                {"Invoices_Issue", "Issue invoice"},
                {"Settings_ManageUsers", "Manage users"},
                {"Settings_ChangeTheme", "Change theme"},
                {"Settings_ChangeLanguage", "Change language"},
                {"Button_AddUser", "➕ Add user"}
            }
        };

        private static string _currentLanguage = "sr";
        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    Instance?.OnPropertyChanged(nameof(GetString));
                }
            }
        }

        public static LocalizationManager Instance { get; } = new LocalizationManager();

        public string GetString(string key)
        {
            if (_resources.TryGetValue(_currentLanguage, out var dict) && dict.TryGetValue(key, out var value))
                return value;
            return $"[{key}]";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
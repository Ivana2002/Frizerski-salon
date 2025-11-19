// Core/Services/DatabaseService.cs
using IvanaDrugi.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace IvanaDrugi.Core.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["SalonDB"].ConnectionString;
        }

        public List<Usluga> GetSveUsluge()
        {
            var usluge = new List<Usluga>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM usluge WHERE aktivan = TRUE";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usluge.Add(new Usluga
                            {
                                UslugaId = reader.GetInt32("usluga_id"),
                                Sifra = reader.GetString("sifra"),
                                Naziv = reader.GetString("naziv"),
                                Opis = reader.IsDBNull("opis") ? null : reader.GetString("opis"),
                                Cena = reader.GetDecimal("cena"),
                                TrajanjeMinuta = reader.GetInt32("trajanje_minuta"),
                                Aktivan = reader.GetBoolean("aktivan"),
                                KategorijaId = reader.GetInt32("kategorija_id")
                            });
                        }
                    }
                }
            }

            return usluge;
        }

        public void DodajUslugu(Usluga usluga)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO usluge (sifra, naziv, opis, cena, trajanje_minuta, kategorija_id, aktivan)
                    VALUES (@sifra, @naziv, @opis, @cena, @trajanje, @kategorijaId, @aktivan)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sifra", usluga.Sifra);
                    cmd.Parameters.AddWithValue("@naziv", usluga.Naziv);
                    cmd.Parameters.AddWithValue("@opis", usluga.Opis ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cena", usluga.Cena);
                    cmd.Parameters.AddWithValue("@trajanje", usluga.TrajanjeMinuta);
                    cmd.Parameters.AddWithValue("@kategorijaId", usluga.KategorijaId);
                    cmd.Parameters.AddWithValue("@aktivan", usluga.Aktivan);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ObrisiUslugu(int uslugaId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE usluge SET aktivan = FALSE WHERE usluga_id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", uslugaId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Korisnik Prijava(string korisnickoIme, string lozinka)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Povezano  na bazu. Traži se  korisnik: " + korisnickoIme);

                    string query = @"
                SELECT korisnik_id, korisnicko_ime, uloga, preferirana_tema, jezik_koda, ime, prezime
                FROM korisnici 
                WHERE korisnicko_ime = @username AND lozinka_hash = @password";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", korisnickoIme);
                        cmd.Parameters.AddWithValue("@password", lozinka);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Korisnik pronađen u bazi. Uloga: " + reader.GetString("uloga"));
                                return new Korisnik
                                {
                                    KorisnikId = reader.GetInt32("korisnik_id"),
                                    KorisnickoIme = reader.GetString("korisnicko_ime"),
                                    Uloga = reader.GetString("uloga"),
                                    PreferiranaTema = reader.GetString("preferirana_tema"),
                                    JezikKoda = reader.GetString("jezik_koda"),
                                    Ime = reader.GetString("ime"),
                                    Prezime = reader.GetString("prezime")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri prijavi: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("Nije pronađen korisnik (pogrešno korisničko ime/lozinka).");
            return null;
        }


        public void DodajKorisnika(Korisnik korisnik)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO korisnici (korisnicko_ime, lozinka_hash, uloga, ime, prezime, preferirana_tema, jezik_koda)
            VALUES (@korisnicko_ime, @lozinka_hash, @uloga, @ime, @prezime, @preferirana_tema, @jezik_koda)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@korisnicko_ime", korisnik.KorisnickoIme);
                    cmd.Parameters.AddWithValue("@lozinka_hash", korisnik.LozinkaHash);
                    cmd.Parameters.AddWithValue("@uloga", korisnik.Uloga);
                    cmd.Parameters.AddWithValue("@ime", korisnik.Ime);
                    cmd.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    cmd.Parameters.AddWithValue("@preferirana_tema", korisnik.PreferiranaTema);
                    cmd.Parameters.AddWithValue("@jezik_koda", korisnik.JezikKoda);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Rezervacija> GetSveRezervacije()
        {
            var rezervacije = new List<Rezervacija>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.*, k.puno_ime, k.telefon
            FROM rezervacije r
            LEFT JOIN klijenti k ON r.klijent_id = k.klijent_id";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var r = new Rezervacija
                        {
                            RezervacijaId = reader.GetInt32("rezervacija_id"),
                            KlijentId = reader.GetInt32("klijent_id"),
                            DodeljenKorisnikId = reader.GetInt32("dodeljen_korisnik_id"),
                            DatumVreme = reader.GetDateTime("datum_vreme"),
                            Status = reader.GetString("status"),
                            Kreirano = reader.GetDateTime("kreirano"),
                            Klijent = new Klijent
                            {
                                PunoIme = reader.IsDBNull("puno_ime") ? "Nepoznat" : reader.GetString("puno_ime"),
                                Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon")
                            }
                        };
                        r.Usluge = GetUslugeZaRezervaciju(r.RezervacijaId);
                        rezervacije.Add(r);
                    }
                }
            }
            return rezervacije;
        }



        public List<Racun> GetSveRacune()
        {
            var racuni = new List<Racun>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.racun_id, r.broj_racuna, r.datum_izdavanja, r.ukupan_iznos, 
                   r.klijent_id, r.rezervacija_id, r.izdao_korisnik_id,
                   rez.klijent_id as rez_klijent_id
            FROM racuni r
            LEFT JOIN rezervacije rez ON r.rezervacija_id = rez.rezervacija_id";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var r = new Racun
                        {
                            RacunId = reader.GetInt32("racun_id"),
                            BrojRacuna = reader.GetString("broj_racuna"),
                            DatumIzdavanja = reader.GetDateTime("datum_izdavanja"),
                            UkupnaCijena = reader.GetDecimal("ukupan_iznos"),
                            KlijentId = reader.IsDBNull("klijent_id") ? 0 : reader.GetInt32("klijent_id"),
                            RezervacijaId = reader.IsDBNull("rezervacija_id") ? 0 : reader.GetInt32("rezervacija_id"),
                            IzdaoKorisnikId = reader.IsDBNull("izdao_korisnik_id") ? 0 : reader.GetInt32("izdao_korisnik_id")
                        };

                        
                        int klijentId = r.KlijentId;
                        if (klijentId == 0 && !reader.IsDBNull("rez_klijent_id"))
                        {
                            klijentId = reader.GetInt32("rez_klijent_id");
                        }

                        if (klijentId > 0)
                            r.Klijent = GetKlijent(klijentId);
                        else
                            r.Klijent = new Klijent { PunoIme = "Nepoznat" };

                        racuni.Add(r);
                    }
                }
            }
            return racuni;
        }


        public void IzdajRacun(Racun racun)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO racuni (broj_racuna, datum_izdavanja, ukupan_iznos, rezervacija_id, izdao_korisnik_id)
            VALUES (@broj_racuna, @datum_izdavanja, @ukupan_iznos, @rezervacija_id, @izdao_korisnik_id)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@broj_racuna", racun.BrojRacuna);
                    cmd.Parameters.AddWithValue("@datum_izdavanja", racun.DatumIzdavanja);
                    cmd.Parameters.AddWithValue("@ukupan_iznos", racun.UkupnaCijena);
                    cmd.Parameters.AddWithValue("@rezervacija_id", racun.RezervacijaId);
                    cmd.Parameters.AddWithValue("@izdao_korisnik_id", racun.IzdaoKorisnikId); 

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SacuvajPreferencu(int korisnikId, string tema, string jezik)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
            UPDATE korisnici 
            SET preferirana_tema = @tema, jezik_koda = @jezik 
            WHERE korisnik_id = @id", conn);

                cmd.Parameters.AddWithValue("@tema", tema);
                cmd.Parameters.AddWithValue("@jezik", jezik);
                cmd.Parameters.AddWithValue("@id", korisnikId);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Rezervacija> GetAktivneRezervacije()
        {
            var rezervacije = new List<Rezervacija>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.rezervacija_id, r.klijent_id, r.dodeljen_korisnik_id, 
                   r.datum_vreme, r.status, r.kreirano,
                   k.puno_ime, k.telefon
            FROM rezervacije r
            JOIN klijenti k ON r.klijent_id = k.klijent_id
            WHERE r.status = 'u_toku'";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rez = new Rezervacija
                        {
                            RezervacijaId = reader.GetInt32("rezervacija_id"),
                            KlijentId = reader.GetInt32("klijent_id"),
                            DodeljenKorisnikId = reader.GetInt32("dodeljen_korisnik_id"),
                            DatumVreme = reader.GetDateTime("datum_vreme"),
                            Status = reader.GetString("status"),
                            Kreirano = reader.GetDateTime("kreirano"),
                            Klijent = new Klijent
                            {
                                PunoIme = reader.GetString("puno_ime"),
                                Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon")
                            },
                            
                        };
                        rezervacije.Add(rez);
                    }
                }
            }

            
            foreach (var r in rezervacije)
            {
                r.Usluge = GetUslugeZaRezervaciju(r.RezervacijaId);
             
            }

            return rezervacije;
        }

        public  List<Usluga> GetUslugeZaRezervaciju(int rezervacijaId)
        {
            var usluge = new List<Usluga>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT u.*
            FROM usluge_rezervacije ur
            JOIN usluge u ON ur.usluga_id = u.usluga_id
            WHERE ur.rezervacija_id = @rezId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@rezId", rezervacijaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usluge.Add(new Usluga
                            {
                                UslugaId = reader.GetInt32("usluga_id"),
                                Sifra = reader.GetString("sifra"),
                                Naziv = reader.GetString("naziv"),
                                Cena = reader.GetDecimal("cena"),
                                TrajanjeMinuta = reader.GetInt32("trajanje_minuta")
                            });
                        }
                    }
                }
            }
            return usluge;
        }

        public void ZavrsiRezervaciju(int rezervacijaId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE rezervacije SET status = 'zavrseno' WHERE rezervacija_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", rezervacijaId);
                cmd.ExecuteNonQuery();
            }
        }


        public Klijent GetKlijent(int klijentId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM klijenti WHERE klijent_id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", klijentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Klijent
                            {
                                KlijentId = reader.GetInt32("klijent_id"),
                                PunoIme = reader.GetString("puno_ime"),
                                Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon"),
                                Email = reader.IsDBNull("email") ? null : reader.GetString("email")
                            };
                        }
                    }
                }
            }
            return new Klijent { PunoIme = "Nepoznat" };
        }


        public List<Racun> GetRacunePoDatumu(DateTime datum)
        {
            var racuni = new List<Racun>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.racun_id, r.broj_racuna, r.datum_izdavanja, r.ukupan_iznos, 
                   r.klijent_id, r.rezervacija_id, r.izdao_korisnik_id,
                   rez.klijent_id as rez_klijent_id
            FROM racuni r
            LEFT JOIN rezervacije rez ON r.rezervacija_id = rez.rezervacija_id
            WHERE DATE(r.datum_izdavanja) = @datum";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@datum", datum.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = new Racun
                            {
                                RacunId = reader.GetInt32("racun_id"),
                                BrojRacuna = reader.GetString("broj_racuna"),
                                DatumIzdavanja = reader.GetDateTime("datum_izdavanja"),
                                UkupnaCijena = reader.GetDecimal("ukupan_iznos"),
                                KlijentId = reader.IsDBNull("klijent_id") ? 0 : reader.GetInt32("klijent_id"),
                                RezervacijaId = reader.IsDBNull("rezervacija_id") ? 0 : reader.GetInt32("rezervacija_id"),
                                IzdaoKorisnikId = reader.IsDBNull("izdao_korisnik_id") ? 0 : reader.GetInt32("izdao_korisnik_id")
                            };

                            // 👇 PRVO PROVERI KLIJENTA IZ RACUNA
                            if (r.KlijentId > 0)
                            {
                                r.Klijent = GetKlijent(r.KlijentId);
                            }
                            // 👇 AKO NEMA, PROVERI KROZ REZERVACIJU
                            else if (r.RezervacijaId > 0 && !reader.IsDBNull("rez_klijent_id"))
                            {
                                r.Klijent = GetKlijent(reader.GetInt32("rez_klijent_id"));
                            }
                            else
                            {
                                r.Klijent = new Klijent { PunoIme = "Nepoznat" };
                            }

                            racuni.Add(r);
                        }
                    }
                }
            }
            return racuni;
        }
        public List<Klijent> GetSveKlijente()
        {
            var klijenti = new List<Klijent>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM klijenti";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        klijenti.Add(new Klijent
                        {
                            KlijentId = reader.GetInt32("klijent_id"),
                            PunoIme = reader.GetString("puno_ime"),
                            Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon"),
                            Email = reader.IsDBNull("email") ? null : reader.GetString("email")
                        });
                    }
                }
            }
            return klijenti;
        }

        public int DodajKlijenta(Klijent klijent)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO klijenti (puno_ime, telefon, email)
            VALUES (@ime, @telefon, NULL);
            SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ime", klijent.PunoIme);
                    cmd.Parameters.AddWithValue("@telefon", klijent.Telefon ?? (object)DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Rezervacija> GetRezervacijePoDatumu(DateTime datum)
        {
            var rezervacije = new List<Rezervacija>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.*, k.puno_ime, k.telefon
            FROM rezervacije r
            JOIN klijenti k ON r.klijent_id = k.klijent_id
            WHERE DATE(r.datum_vreme) = @datum";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@datum", datum.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rezervacije.Add(new Rezervacija
                            {
                                RezervacijaId = reader.GetInt32("rezervacija_id"),
                                KlijentId = reader.GetInt32("klijent_id"),
                                DodeljenKorisnikId = reader.GetInt32("dodeljen_korisnik_id"),
                                DatumVreme = reader.GetDateTime("datum_vreme"),
                                Status = reader.GetString("status"),
                                Kreirano = reader.GetDateTime("kreirano"),
                                Klijent = new Klijent
                                {
                                    PunoIme = reader.GetString("puno_ime"),
                                    Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon")
                                }
                            });
                        }
                    }
                }
            }

            
            foreach (var r in rezervacije)
            {
                r.Usluge = GetUslugeZaRezervaciju(r.RezervacijaId);
            }

            return rezervacije;
        }


        public int DodajRezervaciju(Rezervacija rezervacija)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO rezervacije (klijent_id, dodeljen_korisnik_id, datum_vreme, status, kreirano)
            VALUES (@klijent_id, @dodeljen_korisnik_id, @datum_vreme, @status, @kreirano);
            SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@klijent_id", rezervacija.KlijentId);
                    cmd.Parameters.AddWithValue("@dodeljen_korisnik_id", rezervacija.DodeljenKorisnikId);
                    cmd.Parameters.AddWithValue("@datum_vreme", rezervacija.DatumVreme);
                    cmd.Parameters.AddWithValue("@status", rezervacija.Status);
                    cmd.Parameters.AddWithValue("@kreirano", DateTime.Now);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void DodajUsluguZaRezervaciju(int rezervacijaId, int uslugaId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO usluge_rezervacije (rezervacija_id, usluga_id) 
            VALUES (@rezervacija_id, @usluga_id)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@rezervacija_id", rezervacijaId);
                    cmd.Parameters.AddWithValue("@usluga_id", uslugaId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        
        public List<Rezervacija> GetZakazaneRezervacije()
        {
            var rezervacije = new List<Rezervacija>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.*, k.puno_ime, k.telefon
            FROM rezervacije r
            JOIN klijenti k ON r.klijent_id = k.klijent_id
            WHERE r.status = 'zakazano'";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rez = new Rezervacija
                        {
                            RezervacijaId = reader.GetInt32("rezervacija_id"),
                            KlijentId = reader.GetInt32("klijent_id"),
                            DodeljenKorisnikId = reader.GetInt32("dodeljen_korisnik_id"),
                            DatumVreme = reader.GetDateTime("datum_vreme"),
                            Status = reader.GetString("status"),
                            Kreirano = reader.GetDateTime("kreirano"),
                            Klijent = new Klijent
                            {
                                PunoIme = reader.GetString("puno_ime"),
                                Telefon = reader.IsDBNull("telefon") ? null : reader.GetString("telefon")
                            }
                        };
                        rez.Usluge = GetUslugeZaRezervaciju(rez.RezervacijaId);
                        rezervacije.Add(rez);
                    }
                }
            }
            return rezervacije;
        }

       
        public void ZapocniRezervaciju(int rezervacijaId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE rezervacije SET status = 'u_toku' WHERE rezervacija_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", rezervacijaId);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Korisnik> GetSveKorisnike()
        {
            var korisnici = new List<Korisnik>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM korisnici";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        korisnici.Add(new Korisnik
                        {
                            KorisnikId = reader.GetInt32("korisnik_id"),
                            KorisnickoIme = reader.GetString("korisnicko_ime"),
                            Ime = reader.GetString("ime"),
                            Prezime = reader.GetString("prezime"),
                            Uloga = reader.GetString("uloga"),
                            PreferiranaTema = reader.GetString("preferirana_tema"),
                            JezikKoda = reader.GetString("jezik_koda")
                        });
                    }
                }
            }
            return korisnici;
        }

        public void ObrisiKorisnika(int korisnikId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM korisnici WHERE korisnik_id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", korisnikId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AzurirajKorisnika(Korisnik korisnik)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            UPDATE korisnici 
            SET korisnicko_ime = @korisnicko_ime, 
                uloga = @uloga, 
                ime = @ime, 
                prezime = @prezime, 
                preferirana_tema = @tema, 
                jezik_koda = @jezik
            WHERE korisnik_id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@korisnicko_ime", korisnik.KorisnickoIme);
                    cmd.Parameters.AddWithValue("@uloga", korisnik.Uloga);
                    cmd.Parameters.AddWithValue("@ime", korisnik.Ime);
                    cmd.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    cmd.Parameters.AddWithValue("@tema", korisnik.PreferiranaTema);
                    cmd.Parameters.AddWithValue("@jezik", korisnik.JezikKoda);
                    cmd.Parameters.AddWithValue("@id", korisnik.KorisnikId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
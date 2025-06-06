using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TP4___Calendrier
{
    internal class Calendrier
    {
        enum NomsMois {Janvier = 1, Fevrier, Mars, Avril, Mai, Juin,
            Juillet, Aout, Septembre, Octobre, Novembre, Decembre };

        static NomsMois? Mois { get; set; }

        private readonly static string[] Jours = { "Lu", "Ma", "Me", "Je", "Ve", "Sa", "Di" };

        static string[]? NombreJours { get; set; }

        static public void LancerProgramme(string[] args)
        {
            ValiderArguments(args, out int numValidation);
            AfficherMois(numValidation);
        }

        static private void ValiderArguments(string[] args, out int numValidation)
        {
            string argMois = "";
            numValidation = -1;
            bool aideDemandee = false;

            if (args.Length > 0)
            {
                //Verfie si help
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--help" | args[i] == "-h")
                    {
                        aideDemandee = true;
                    }
                }
                if (!aideDemandee)
                {
                    //Recherche argument mois
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "--mois" | args[i] == "-m")
                        {
                            if (args.Length > 1 && args.Length <= 2)
                            {
                                argMois = args[++i];

                                if (Enum.TryParse(typeof(NomsMois), argMois, true, out object? nomMois) && Enum.IsDefined(typeof(NomsMois), nomMois))
                                {
                                    numValidation = (int)nomMois;
                                }
                                //Si erreur
                                else
                                {
                                    numValidation = -1;
                                }
                            }
                            //Si rien apres --mois ou -m
                            else
                            {
                                numValidation = -1;
                            }
                        }
                        //Si pas --mois ni -m
                        else
                        {
                            numValidation = -1;
                        }
                    }
                }
                //Aide demandée true
                else
                {
                    numValidation = 0;
                }
            }
            //Si ni help ni arguments
            else
            {
                numValidation = -1;
            }
        }

        static private void AfficherMois(int numValidation)
        {
            if (numValidation == 0)
            {
                AfficherAide();
            }
            else if (numValidation == -1)
            {
                DemanderMois();
                DeterminerJoursMois();
                AffichageJourCommencement(out string espacement, out string arret);
                AfficherCalendrier(espacement, arret);
            }
            else if (numValidation >= 1 && numValidation <= 12)
            {
                Mois = (NomsMois)(numValidation);
                DeterminerJoursMois();
                AffichageJourCommencement(out string espacement, out string arret);
                AfficherCalendrier(espacement, arret);
            }
            else /*Si pas d'arguments*/
            {
                DeterminerJoursMois();
                AffichageJourCommencement(out string espacement, out string arret);
                AfficherCalendrier(espacement, arret);
            }
        }

        static private void AfficherAide()
        {
            Console.WriteLine("Instruction d'utilisation: " +
                "\n\n\tPour entrer des arguments : inscrivez -m ou --mois pour le mois" +
                "\n\tsuivi du chiffre du mois ou du nom du mois (en commencant par une majuscule)" +
                "\n\tPar exemple : \"--mois Decembre\"" +
                "\n\n\tPour afficher le menu aide : inscrivez -h ou --help");
        }

        static private void DemanderMois()
        {
            string? saisieUser;
     
            bool moisValide = false;

            do
            {
                Console.WriteLine("Quel mois de l'annee 2025 souhaitez-vous afficher ? (numero ou nom)");

                saisieUser = Console.ReadLine();

                if (saisieUser != null)
                {
                    //Utilisateur rentre un numero
                    if (int.TryParse(saisieUser, out int numMois) && numMois >= 1 && numMois <= 12)
                    {
                        Mois = (NomsMois)(numMois);
                        moisValide = true;
                    }
                    //Utilisateur rentre une chaine
                    else if (!int.TryParse(saisieUser, out _))
                    {
                        saisieUser = char.ToUpper(saisieUser[0]) + saisieUser.Substring(1).ToLower();

                        if (Enum.TryParse(typeof(NomsMois), saisieUser, out object? nomMois))
                        {
                            Mois = (NomsMois)nomMois;
                            moisValide = true;
                        }
                    }
                }
            } while (!moisValide == true);
        }

        static private void DeterminerJoursMois()
        {
            if (Mois == NomsMois.Fevrier)
            {
                NombreJours = Enumerable.Range(1, 28).Select(nombre => nombre.ToString()).ToArray();
            }
            else if (Mois == NomsMois.Avril || Mois == NomsMois.Juin || Mois == NomsMois.Septembre || Mois == NomsMois.Novembre)
            {
                NombreJours = Enumerable.Range(1, 30).Select(nombre => nombre.ToString()).ToArray();
            }
            else
            {
                NombreJours = Enumerable.Range(1, 31).Select(nombre => nombre.ToString()).ToArray();
            }
        }

        static private void AffichageJourCommencement(out string espacement, out string arret)
        {
            if (Mois == NomsMois.Septembre || Mois == NomsMois.Decembre) //Lundi
            {
                espacement = "";
                arret = NombreJours[6];
            }
            else if (Mois == NomsMois.Avril || Mois == NomsMois.Juillet) //Mardi
            {
                espacement = "  ";
                arret = NombreJours[5];
            }
            else if (Mois == NomsMois.Janvier || Mois == NomsMois.Octobre) //Mercredi
            {
                espacement = "   ";
                arret = NombreJours[4];
            }
            else if (Mois == NomsMois.Mai) //Jeudi
            {
                espacement = "    ";
                arret = NombreJours[3];
            }
            else if (Mois == NomsMois.Aout) //Vendredi
            {
                espacement = "     ";
                arret = NombreJours[2];
            }
            else if (Mois == NomsMois.Juin) //Dimanche
            {
                espacement = "       ";
                arret = NombreJours[0];
            }
            else//Samedi
            {
                espacement = "         ";
                arret = NombreJours[1];
            }
        }

        static private void AfficherCalendrier(string espacement, string arret)
        {

            Console.WriteLine($"\n{Mois} 2025" +
            $"\n{string.Join(" ", Jours)}" +
            $"\n{espacement}");

            for (int i = 0; i < NombreJours.Length; i++)
            {
                if (NombreJours[i] == arret)
                {
                    Console.Write($" {NombreJours[i]}\n");
                }
                else
                {
                    Console.Write($" {NombreJours[i]}");
                }
            }


            //BONNE VOIE ajuster les espacements dans affichagejourcommencement et les jours d arrets





            //Determiner comment afficher le mois, genre tous les 7 jours faire \n

            //for (int i = 0; i < NombreJours?.Length - 1; i++)
            //{

            //    if (NombreJours?[i] == NombreJours?[7] || NombreJours?[i] == NombreJours?[14] || NombreJours[i] == NombreJours[21] || NombreJours[i] == NombreJours[28])
            //    {
            //        Console.Write($"\n  {string.Join("", NombreJours[i])}");
            //    }
            //    //voir le nombre d espace apres
            //    else
            //    {
            //        Console.Write($"  {string.Join("", NombreJours[i])}");
            //    }
            //}
            //Console.WriteLine();

            //string[] joursEtMois = new string[NombreJours];

            //for (int i = 0; i < NombreJours?.Length; i++)
            //{
            //    NombreJours[i] = Jours[i % 7];
            //}

            //Console.Write(string.Join(" ", NombreJours));
        }
    }
}

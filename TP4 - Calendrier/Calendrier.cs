using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TP4___Calendrier
{
    internal class Calendrier
    {
        enum NomsMois {Janvier, Fevrier, Mars, Avril, Mai, Juin,
            Juillet, Aout, Septembre, Octobre, Novembre, Decembre };

        static NomsMois? Mois { get; set; } //Quel statut lui donner ? Est ce bon ?

                 // Champ privé en lecture seule
        private static readonly string[] jours = { "Lu", "Ma", "Me", "Je", "Ve", "Sa", "Di" };

        // Propriété publique en lecture seule
        public IReadOnlyList<string> JourSemaine => jours;

        static public void LancerProgramme(string[] args)
        {
            ValiderArguments(args, out int numValidation);
            AfficherMois(numValidation);
        }

        static void ValiderArguments(string[] args, out int numValidation)
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

                                //Vérifie si numero de mois est entré directement
                                if (int.TryParse(argMois, out int numeroMois) && numeroMois >= 1 && numeroMois <= 12)
                                {
                                    numValidation = numeroMois;
                                }
                                //Vérifie si nom du mois est entré directement
                                else if (Enum.TryParse(typeof(NomsMois), argMois, true, out object? NomMois) && numeroMois != 0)
                                {
                                    numValidation = (int)NomMois + 1;
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

        static void AfficherMois(int numValidation)
        {
            if (numValidation == 0)
            {
                AfficherAide();
            }
            else if (numValidation == -1)
            {
                DemanderMois();
                Console.WriteLine($"Mon mois est {Mois} et le numero de validation {numValidation}");
                // ++ Afficher calendrier
            }
            else if (numValidation >= 1 && numValidation <= 12)
            {
                Mois = (NomsMois)(numValidation - 1);
                Console.WriteLine($"Mon mois est {Mois} et le numero de validation {numValidation}");
                // ++ Afficher calendrier
            }
            else
            {
                Console.WriteLine($"Mon mois est {Mois} et le numero de validation {numValidation}");
                // ++ Afficher calendrier
            }
        }

        static void AfficherAide()
        {
            Console.WriteLine("Instruction d'utilisation: " +
                "\n\n\tPour entrer des arguments : inscrivez -m ou --mois pour le mois" +
                "\n\tsuivi du chiffre du mois ou du nom du mois (en commencant par une majuscule)" +
                "\n\tPar exemple : \"--mois Decembre\"" +
                "\n\n\tPour afficher le menu aide : inscrivez -h ou --help");
        }

        static void DemanderMois()
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
                        Mois = (NomsMois)(numMois - 1);
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
    }
}

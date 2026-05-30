using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using LibOrbito;

namespace OrbitoPersistance
{
    public static class Persistance
    {
        public static Historique ChargerHistorique(string xmlFile, Historique hist)
        {
            if (File.Exists(xmlFile))
            {
                Console.WriteLine("Chargement des données en cours...");
                try
                {
                    var serializer = new DataContractSerializer(typeof(LibOrbito.Historique));
                    using (Stream s = File.OpenRead(xmlFile))
                    {
                        LibOrbito.Historique? histori = serializer.ReadObject(s) as LibOrbito.Historique;
                        if (histori != null)
                        {
                            hist = histori;
                            Console.WriteLine("Chargement des données effectué avec succès !");
                        }
                        return hist;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"\n\n[ERREUR DE CHARGEMENT XML] : {ex.Message}\n\n");
                    Console.WriteLine($"Erreur : {ex.Message}");
                    return hist;
                }
            }
            else
            {
                Console.WriteLine("Aucun fichier XML trouvé, chargement du Stub.");
                return Stub.StubGenerator.CreerHistoriqueBouchon();
            }
        }

        public static void SauvegarderHistorique(Historique historique, string xmlFile)
        {
            Console.WriteLine("Sauvegarde en cours...");
            try
            {
                var serializer = new DataContractSerializer(typeof(LibOrbito.Historique));
                string? directory = Path.GetDirectoryName(xmlFile);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(xmlFile, settings))
                {
                    serializer.WriteObject(writer, historique);
                }
                Console.WriteLine("Fin sauvegarde");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Erreur Sauvegarde Historique] : {ex.Message}");
            }
        }

        public static Partie? ChargerPartieEnCours(string xmlSauvegarde)
        {
            if (File.Exists(xmlSauvegarde))
            {
                try
                {
                    var serializer = new DataContractSerializer(typeof(LibOrbito.Partie));
                    using (Stream s = File.OpenRead(xmlSauvegarde))
                    {
                        var p = serializer.ReadObject(s) as LibOrbito.Partie;

                        if (p != null && p.Joueur1 != null && p.Joueur1.Pseudo != null)
                        {
                            return p;
                        }
                        else
                        {
                            throw new Exception("Partie corrompue détectée (données vides)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERREUR CHARGEMENT : {ex.Message}");
                    File.Delete(xmlSauvegarde);
                    return null;
                }
            }
            return null;
        }

        public static void SauvegarderPartieEnCours(Partie? partieActive, string xmlSauvegarde)
        {
            if (partieActive == null || partieActive.EstTerminee())
            {
                if (File.Exists(xmlSauvegarde)) File.Delete(xmlSauvegarde);
                return;
            }

            try
            {
                var serializer = new DataContractSerializer(typeof(LibOrbito.Partie));
                string? directory = Path.GetDirectoryName(xmlSauvegarde);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (XmlWriter writer = XmlWriter.Create(xmlSauvegarde, new XmlWriterSettings { Indent = true }))
                {
                    serializer.WriteObject(writer, partieActive);
                }
                System.Diagnostics.Debug.WriteLine("Sauvegarde bien effectuée pour ce coup");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Erreur Auto-Save] : {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Constantes
{
    public enum TypeRecherche
    {
        AutoCompletion = 1,
        ListeDeroulante = 2
    }

    public enum TypeLieuFonctionnel
    {
        Paroisse = 1,
    }
    
    public enum TypeActe
    {
        Bapteme = 1,
        Mariage = 2,
        Sepulture = 3
    }

    public enum TypeMedium
    {
        Document = 1,
        Video = 2,
        Photo = 3,
        Son = 4,
        NonMedia = 5
    }

    public enum TypeColonneFond
    {
        Chaine = 1,
        Entier = 2,
        Date = 3
    }

    public enum TypeVueFond
    {
        DefautMedia = 1,
        ImageMiniature = 2
    }

    public enum TypeColonneOperateur
    {
        Egal = 1,
        Contient = 2,
        Superieur = 3,
        SuperieurOuEgal = 4,
        Inferieur = 5,
        InferieurOuEgal = 6,
        Different = 7
    }
}

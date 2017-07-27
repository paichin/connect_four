using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public enum Resultat { j1gagne, j0gagne, partieNulle, indetermine }

class MainClass
{
    public static void Main(string[] args)
    {
        float a = 5;
        int temps = 1000;
        bool j1aletrait = true;
        PositionP4 pInitiale = new PositionP4(j1aletrait);
        Joueur j1 = new JoueurHumainPuissance();
        JoueurMCTS j2 = new JoueurMCTS(a,temps);
        Partie jeu = new Partie(j1, j2, pInitiale);
        Console.WriteLine("\t\tCommencer a jouer le jeu\n\n "); //begin to play the game
        jeu.Commencer();
        Console.ReadKey();
    }
}
public abstract class Position
{
    public bool j1aletrait;
    public Position(bool j1aletrait) { this.j1aletrait = j1aletrait; }
    public abstract Resultat Eval();
    public abstract int NbCoups { get;  set; }
    public abstract Position PositionFille(int i);
    public abstract void Affiche();
}
public class PositionP4 : Position
{
    public PositionP4(bool j1aletrait): base(j1aletrait){}
    const int nbLigne = 6;
    const int nbCol = 7;
    public int[,] tab = new int[nbLigne, nbCol];
    public int color;
    
    public override bool Equals(object obj)
    {
        return true;
    }

    public override int NbCoups {
        get{
             int res = 7;
              for (int i = 0; i < nbCol; i++)
             {
                    if (tab[0, i] == 1 || tab[0, i] == 2)
                   {
                      res = res - 1;
                    }
                }
                return res;
            }
    set{this.NbCoups= value;}
    }

    public override Resultat Eval()
    {
        int i = 0, j = 0, lj1 = 0, lj2 = 0;
        for (i = 0;i < nbLigne; i++)
        {
            lj1 = 0;lj2 = 0;
            for (j = 0;j < nbCol; j++)
            {
                switch (tab[i, j])
                {
                    case (1):
                        lj1++;if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }
        for (j = 0;j < nbCol; j++)
        {
            lj1 = 0;lj2 = 0;
            for (i = 0;i < nbLigne; i++)
            {
                switch (tab[i, j])
                {
                    case (1):
                        lj1++;
                        if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;
                        if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }
        for (i = 0;i < nbLigne; i++)
        {
            lj1 = 0; lj2 = 0;
            for (j = 0;j < nbCol && i + j < nbLigne; j++)
            {
                switch (tab[i + j, j])
                {
                    case (1):
                        lj1++;if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }
        for (j = 1;j< nbCol; j++)
        {
            lj1 = 0;lj2 = 0;
            for (i = 0;i < nbLigne && i + j < nbCol; i++)
            {
                switch (tab[i, i + j])
                {
                    case (1):
                        lj1++;if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }
        for (i = 0;i< nbLigne; i++)
        {
            lj1 = 0;lj2 = 0;
            for (j = 0;j < nbCol && i >= j; j++)
            {
                switch (tab[i - j, j])
                {
                    case (1):
                        lj1++;if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }

        for (j = 1;j < nbCol; j++)
        {
            lj1 = 0;lj2 = 0;
            for (i=0;i<nbLigne && j+i< nbCol; i++)
            {
                switch (tab[nbLigne - i - 1, j + i])
                {
                    case (1):
                        lj1++;if (lj1 >= 4) return (Resultat.j1gagne);
                        lj2 = 0;
                        break;
                    case (2):
                        lj2++;if (lj2 >= 4) return (Resultat.j0gagne);
                        lj1 = 0;
                        break;
                    case (0):
                        lj1 = 0;
                        lj2 = 0;
                        break;
                }
            }
        }
        return (Resultat.indetermine);
    }

    public override Position PositionFille(int i)
    {
        PositionP4 pf = new PositionP4(this.j1aletrait);
        pf.tab = this.tab;
        i = i - 1;
        int j0 = nbLigne-1;
        while (pf.tab[j0,i] != 0) { j0--; }
        if (j1aletrait) { pf.tab[j0, i] = 2; }
        else { pf.tab[j0, i] = 1; }
        if (pf.j1aletrait == true) { pf.j1aletrait = false; }
        else { pf.j1aletrait = true; }
        return pf;
    }

    public override void Affiche()
    {
        Console.Write("\t|   |");
        for (int k = 0; k < nbCol; k++)
        {
            Console.Write(" ");
            Console.Write(k + 1);
            Console.Write(" |");
        }
        Console.WriteLine("\n");
        for (int i = 0; i < nbLigne; i++)
        {
            Console.Write("\t| ");
            Console.Write(i + 1);
            Console.Write(" |");
            for (int j = 0; j < nbCol; j++)
            {
                Console.Write(" " + tab[i, j] + " |");
            }
            Console.WriteLine(" \n");
        }
    }
     /*   for (int i=0;i< nbLigne; i++)
        {
            for (int j=0;j< nbCol; j++)
            {
                Console.Write(tab[i, j] + " ");
            }
            Console.WriteLine(" ");
        }
    }*/
}

public class JoueurHumainPuissance : Joueur
{
    public override int Jouer(Position p)
    {
        Console.WriteLine("Entrez la colonne");
        int a = int.Parse(Console.ReadLine());
        return a;
    }
}
public class Partie
{
    Position pCourante;
    Joueur j1, j0;
    public Resultat r;
    public Partie(Joueur j1, Joueur j0, Position pInitiale)
    {
        this.j1 = j1;
        this.j0 = j0;
        pCourante = pInitiale;
    }
    public void Commencer()
    {
        do
        {
            pCourante.Affiche();
            if (pCourante.j1aletrait)
            {
                pCourante = pCourante.PositionFille(j1.Jouer(pCourante));
            }
            else
            {
                pCourante = pCourante.PositionFille(j0.Jouer(pCourante));
            }
            r = pCourante.Eval();
        } while (r == Resultat.indetermine);
        pCourante.Affiche();
        switch (r)
        {
            case Resultat.j1gagne: Console.WriteLine("j1 a gagné."); break;
            case Resultat.j0gagne: Console.WriteLine("j0 a gagné."); break;
            case Resultat.partieNulle: Console.WriteLine("Partie nulle."); break;
        }
    }
}
public abstract class Joueur
{
    public abstract int Jouer(Position p);
}
public class Noeud
{
    public Position p;
    public Noeud pere;
    public Noeud[] fils;
    public int cross, win;
    public int indiceMeilleurFils;
    public Noeud(Noeud pere, Position p)
    {
        this.pere = pere;
        this.p = p;
        fils = new Noeud[p.NbCoups];
    }
    public void CalculMeilleurFils(float a)
    {
        indiceMeilleurFils = JoueurMCTS.gen.Next(fils.Length);
        float s;
        if (p.j1aletrait)
        {
            float sM = fils[indiceMeilleurFils] == null ? 1F :
            (fils[indiceMeilleurFils].win + a) / (fils[indiceMeilleurFils].cross + a);
            for (int i = 0; i < fils.Length; i++)
            {
                s = fils[i] == null ? 1F : (fils[i].win + a) / (fils[i].cross + a);
                if (s > sM) { sM = s; indiceMeilleurFils = i; }
            }
        }
        else
        {
            float sM = fils[indiceMeilleurFils] == null ? 1F :
            ((fils[indiceMeilleurFils].cross - fils[indiceMeilleurFils].win) + a) /
            (fils[indiceMeilleurFils].cross + a);
            for (int i = 0; i < fils.Length; i++)
            {
                s = fils[i] == null ? 1F :
                    ((fils[i].cross - fils[i].win + a)) / (fils[i].cross + a);
                if (s > sM) { sM = s; indiceMeilleurFils = i; }
            }
        }
    }
    public Noeud MeilleurFils()
    {
        return InstancierFils(indiceMeilleurFils);
    }
    public Noeud InstancierFils(int i)
    {
        if (fils[i] != null) { return fils[i]; }
        fils[i] = new Noeud(this, p.PositionFille(i));
        return fils[i];
    }
    /*public override string ToString()
    {
        string s = p.j1aletrait ? "T1" : "T0"; s = s + " " + p.Eval();
        s = s + " indice MF = " + indiceMeilleurFils + "\n";
        for (int k = 0; k < fils.Length; k++)
        {
            if (fils[k] != null)
            {
                nb++;
                s += (fils[k].win + "/" + fils[k].cross + " ");
            }
            else s += (0 + "/" + 0 + " ");
        }
        return s;
    }*/
}
public class JoueurMCTS : Joueur
{
    public static Random gen = new Random();
    float a;
    int n;
    int Temps;
    Noeud racine;
    //public JoueurMCTS(float a, int n) { this.a = a; this.n = n; }
    public JoueurMCTS(float a, int Temps) { this.a = a;this.Temps = Temps; }
    Resultat JeuHasard(Position p)
    {
        Resultat res = p.Eval();
        while (res == Resultat.indetermine)
        {
            p = p.PositionFille(gen.Next(0, p.NbCoups));
            res = p.Eval();
        }
        return res;
    }
    public override int Jouer(Position p)
    {
        racine = new Noeud(null, p);
        Stopwatch t0 = Stopwatch.StartNew();
        for (int i = 0; i < n; i++)
        {
            // Sélection
            Noeud no = racine;
            //no.cross = 0;
            do
            {
                no.CalculMeilleurFils(a);
                no = no.MeilleurFils();
            } while (no.fils.Length > 0 && no.cross > 0);
            // Simulation
            Resultat res = JeuHasard(no.p);
            int re = p.j1aletrait ? 0 : 1;
            if (res == Resultat.j1gagne) { re = 1; }
            if (res == Resultat.j0gagne) { re = 0; }
            // Rétropropagation
            while (no != null)
            {
                no.cross += 1;
                no.win += re;
                no = no.pere;
            }
            Temps = (int)t0.ElapsedMilliseconds;
            if (Temps > 1000) { break; }
        }
        racine.CalculMeilleurFils(a);
        Console.WriteLine(racine);
        return racine.indiceMeilleurFils; ;
    }
}
/* Cette classe implémente un exemple très simple de dialogue en utilisant les données du fichier dialogue0.json */
using System;
class McDo : Chatbot
{

    // *********************
    // Si cette variable est à "true", l'agent adopte un comportement négatif, sinon il est "positif"
    private bool badBehavior = true;
    // ********************
    private bool salade = false; // Le sujet vient de commander une salade
    private bool bigmac = false;  // Le sujet vient de commander un bigmac
    private bool sundae = false;  // Le sujet vient de commander un sundae
    private bool ask = false;  // Le sujet demande des informations
    private bool payment = false;  // Le sujet paie
    private bool menu = false;  // Le sujet choisit son menu
    private bool dontKnow = false;  // Le client hésite
    private bool ciao = false; // Le client part
    private int achat = 0; // Le nombre d'achats


    /* implémentation de la méthode getCurrentQuestion() */
    public override int getCurrentQuestion()
    {

        if (ask)
        {
            return badBehavior ? 2 : 13;
        }

        if (dontKnow)
        {

            return badBehavior ? 3 : 12;
        }

        if (bigmac)
        {
            return badBehavior ? 4 : 14;
        }

        if (menu)
        {
            return badBehavior ? 5 : 15;
        }

        if (salade)
        {
            return badBehavior ? 6 : 16;
        }

        if (sundae)
        {
            return badBehavior ? 7 : 17;
        }

        if (ciao)
        {
            if (badBehavior)
            {
                if (achat != 0)
                {
                    return 10;
                }
                else
                {
                    return 9;
                }
            }
            else
            { return 19; }
        }

        if (payment)
        {
            return badBehavior ? 8 : 18;
        }


        return badBehavior ? 1 : 11;

    }


    public override void triggerAffectiveReaction(int lastQuestion, int lastAnswer)
    {


        if (badBehavior != false)
        {

            playAnimation(new int[] { 1, 4, 15 }, new int[] { 100, 100, 100 }, .5f);

        }
        else
        {
            playAnimation(new int[] { 7, 20, 26 }, new int[] { 100, 100, 100 }, .5f);
        }
    }

    /* réécriture de la méthode afterAnswer pour gérer le dialogue */
    public override void afterAnswer(int lastq, int r)
    {
        if (ciao)
        {
            stopDialogue();
        }
        ask = r == 2;
        bigmac = r == 4;
        dontKnow = r == 1;
        payment = r == 3 & achat != 0;
        menu = r == 7 | r == 8;
        salade = r == 5;
        sundae = r == 6;
        if (bigmac | sundae | salade)
        {
            achat += 1;
        }
        ciao = (r == 3 & achat == 0) | (r == 9) | (r == 10);
    }

}
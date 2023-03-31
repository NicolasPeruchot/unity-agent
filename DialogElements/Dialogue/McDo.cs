/* Cette classe implémente un exemple très simple de dialogue en utilisant les données du fichier dialogue0.json */
using System;
class McDo : Chatbot
{

    private bool badBehavior = true;
    private bool salade = false;
    private bool bigmac = false;
    private bool sundae = false;
    private bool ask = false;
    private bool payment = false;
    private bool menu = false;

    private bool dontKnow = false;
    private bool ciao = false; // Le client part
    private int achat = 0; // Le nombre d'achats
    private int n_tacos = 0; // Le nombre de fois où le client à demander un tacos depuis qu'il a fait une commande viable

    // Variables
    public const int AGENT = 0;
    public const int USER = 1;
    public const int OTHER = 2;
    public const int PAST = 0;
    public const int FUTURE = 1;


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

    public double getGoal(int lastQuestion, int lastAnswer)
    {

        // Si le client a fait un achat, on se rapproche du but en lui vendant encore d'autres plats
        // on s'éloigne du but s'il nous demande des tacos
        if (lastAnswer == 4 | lastAnswer == 5 | lastAnswer == 6)
        {
            n_tacos = 0;
            achat += 1;
        }
        return (double)achat - (double)n_tacos / (double)(achat + n_tacos);


    }

    public int getCause(int lastQuestion, int lastAnswer)
    {
        // Si le client commande, l'agent pense que c'est grâce à son contact client
        if (lastAnswer == 2 | lastAnswer == 5 | lastAnswer == 6)
        {
            return AGENT;
        }
        // Si le client veut un tacos, l'agent pense qu'on se moque de lui
        else if (lastAnswer == 1)
        {
            return USER;
        }
        // Si le client n'a rien acheté, l'agent pense être responsable
        else if (lastAnswer == 3)
        {
            return AGENT;
        }
        else
        {
            return OTHER;
        }
    }

    public int getTime(int lastQuestion, int lastAnswer)
    {
        // La temporalité est toujours dans le passé, sauf si le client se moque de nous en demandant un tacos (on pense qu'il continuera)
        if (lastAnswer == 1)
        {
            return FUTURE;
        }
        else
        {
            return PAST;
        }
    }

    public override void triggerAffectiveReaction(int lastQuestion, int lastAnswer)
    {
        int cause = getCause(lastQuestion, lastAnswer);
        double goal = getGoal(lastQuestion, lastAnswer);
        int time = getTime(lastQuestion, lastAnswer);

        if (cause == AGENT | cause == OTHER)
        {
            if (goal > 0)
            // Si l'agent a fait des ventes, il est heureux, sinon non
            {
                playAnimation(new int[] { 6, 12 }, new int[] { 100, 100 }, .5f);
            }
            else
            {
                playAnimation(new int[] { 1, 4, 15 }, new int[] { 100, 100, 100 }, .5f);
            }
        }
        else
        // Le client n'est responsable que des actions négatives: il énerve donc l'agent
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

        ciao = (r == 3 & achat == 0) | (r == 9) | (r == 10);
    }

}
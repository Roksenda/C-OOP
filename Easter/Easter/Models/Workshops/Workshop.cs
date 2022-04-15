using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops.Contracts;

namespace Easter.Models.Workshops
{
    public class Workshop : IWorkshop
    {
        public void Color(IEgg egg, IBunny bunny)
        {
            if (bunny.Energy > 0 && bunny.Dyes.Any(x => !x.IsFinished()))
            {
                IDye dye = bunny.Dyes.First(x => !x.IsFinished());

                while (true)
                {
                    if (egg.IsDone())
                    {
                       break;; 
                    }

                    if (bunny.Energy == 0  || bunny.Dyes.All(x => x.IsFinished()))
                    {
                       break; 
                    }
                    bunny.Work();
                    egg.GetColored();
                    dye.Use();
                   if (dye.IsFinished() && bunny.Dyes.Any(x => !x.IsFinished()))
                   {
                       dye = bunny.Dyes.First(x => !x.IsFinished());

                    }
                }
            }
        }
    }
}

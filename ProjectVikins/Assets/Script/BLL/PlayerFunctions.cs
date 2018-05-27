using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;

namespace Assets.Script.BLL
{
    public class PlayerFunctions : Shared.BLLFunctions<DAL.Player>
    {
        public PlayerFunctions()
            : base("PlayerId")
        {
            SetListContext();
        }
        public override int Create(Player model)
        {
            ListContext.Add(model);
            return model.PlayerId;
        }

        public override void SetListContext()
        {
            this.ListContext = context.GetPlayers();
        }
        
    }
}

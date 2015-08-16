﻿using RimWorld;
using Verse;

namespace CommunityCoreLibrary
{

    public class CompLifespanPowered : ThingComp
    {
        
        CompPowerTrader                     CompPowerTrader
        {
            get
            {
                return parent.TryGetComp< CompPowerTrader >();
            }
        }

        public int                          remainingTicks = -1;

        public override void                PostSpawnSetup()
        {
            base.PostSpawnSetup();

            // Check power comp
#if DEBUG
            if( CompPowerTrader == null )
            {
                Log.Error( "Community Core Library :: CompHeatPusherPowered :: " + parent.def.defName + " requires CompPowerTrader!" );
                return;
            }
#endif

            if( remainingTicks < 0 )
            {
                remainingTicks = props.lifespanTicks;
            }
        }

        public override void                PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.LookValue<int>( ref remainingTicks, "remainingTicks", props.lifespanTicks, true );
        }

        public override void                CompTick()
        {
            base.CompTick();
            TickDown( 1 );
        }

        public override void                CompTickRare()
        {
            base.CompTickRare();
            TickDown( 250 );
        }

        public void                         TickDown( int down )
        {
            if( !CompPowerTrader.PowerOn )
            {
                return;
            }
            
            remainingTicks -= down;
            if( remainingTicks > 0 )
            {
                return;
            }

            parent.Destroy();
        }

        public override string              CompInspectStringExtra()
        {
            if( remainingTicks > 0 )
            {
                return "LifespanExpiry".Translate() + " " +
                    remainingTicks.TickstoDaysAndHoursString() + "\n" +
                    base.CompInspectStringExtra();
            }
            return base.CompInspectStringExtra();
        }

    }

}
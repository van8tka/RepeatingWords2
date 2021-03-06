﻿using Xamarin.Forms;

namespace RepeatingWords.CustomControls
{
    public class ImageButtonAnimationScale : ImageButton
    {
        public ImageButtonAnimationScale():base()
        {          
            this.Clicked += async (sender, e) =>
            {
                var btn = (ImageButtonAnimationScale)sender;
                await btn.ScaleTo(1.2, 250, Easing.SpringIn);
                await btn.ScaleTo(1, 250);
            };
        }               
    }                 
}

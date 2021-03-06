﻿using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Cavokator
{

    internal class NotamFieldsAdapter : RecyclerView.Adapter
    {
        private List<object> mRecyclerNotamList;

        public override int ItemCount => mRecyclerNotamList.Count;

        public NotamFieldsAdapter(List<object> recyclerNotamList)
        {
            mRecyclerNotamList = recyclerNotamList;
        }

        public override int GetItemViewType(int position)
        {
            if (mRecyclerNotamList[position] is MyAirportRecycler)
            {
                return 0;
            }

            if (mRecyclerNotamList[position] is MyNotamCardRecycler)
            {
                return 1;
            }

            if (mRecyclerNotamList[position] is MyErrorRecycler)
            {
                return 2;
            }

            if (mRecyclerNotamList[position] is MyCategoryRecycler)
            {
                return 3;
            }

            return -1;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            RecyclerView.ViewHolder vh;
            LayoutInflater inflater = LayoutInflater.From(parent.Context);

            switch (viewType)
            {
                case 0:
                    View v1 = inflater.Inflate(Resource.Layout.notam_airports, parent, false);
                    vh = new AirportViewHolder(v1);
                    break;

                case 1:
                    View v2 = inflater.Inflate(Resource.Layout.notam_cards, parent, false);
                    vh = new NotamViewHolder(v2);
                    break;

                case 2:
                    View v3 = inflater.Inflate(Resource.Layout.notam_error_card, parent, false);
                    vh = new ErrorViewHolder(v3);
                    break;

                case 3:
                    View v4 = inflater.Inflate(Resource.Layout.notam_category_card, parent, false);
                    vh = new CategoryViewHolder(v4);
                    break;

                default:
                    View vDef = inflater.Inflate(Resource.Layout.notam_error_card, parent, false);
                    vh = new ErrorViewHolder(vDef);
                    break;
            }

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            switch (holder.ItemViewType)
            {
                case 0:
                    AirportViewHolder vh1 = (AirportViewHolder)holder;
                    MyAirportRecycler airport = (MyAirportRecycler)mRecyclerNotamList[position];
                    vh1.AirportNameTextView.Text = airport.Name;
                    break;

                case 1:
                    NotamViewHolder vh2 = (NotamViewHolder)holder;
                    MyNotamCardRecycler notamCard = (MyNotamCardRecycler)mRecyclerNotamList[position];
                    vh2.NotamIdTextView.TextFormatted = notamCard.NotamId;
                    vh2.NotamIdTextView.MovementMethod = new LinkMovementMethod();
                    vh2.NotamFreeTextTextView.Text = notamCard.NotamFreeText;

                    // Remove unnecessary layouts
                    if (notamCard.DisableTopLayout)
                    {
                        vh2.NotamCardMainLayout.RemoveView(vh2.NotamCardTopLayout);
                    }
                    
                    break;

                case 2:
                    ErrorViewHolder vh3 = (ErrorViewHolder)holder;
                    MyErrorRecycler errorCard = (MyErrorRecycler)mRecyclerNotamList[position];
                    vh3.ErrorTextView.Text = errorCard.ErrorString;
                    break;

                case 3:
                    CategoryViewHolder vh4 = (CategoryViewHolder)holder;
                    MyCategoryRecycler categoryCard = (MyCategoryRecycler)mRecyclerNotamList[position];
                    vh4.CategoryTextView.Text = categoryCard.CategoryString;
                    break;
            }
        }
    }

    internal class AirportViewHolder : RecyclerView.ViewHolder
    {
        public TextView AirportNameTextView { get; }

        public AirportViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            AirportNameTextView = itemView.FindViewById<TextView>(Resource.Id.airportCard_name);
            AirportNameTextView.SetTextColor(new ApplyTheme().GetColor(DesiredColor.MagentaText));
        }
    }

    internal class NotamViewHolder : RecyclerView.ViewHolder
    {
        public CardView NotamMainCardView { get; }
        public LinearLayout NotamCardMainLayout { get; }
        public RelativeLayout NotamCardTopLayout { get; }
        public TextView NotamIdTextView { get; }
        public TextView NotamFreeTextTextView { get; }

        public NotamViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            NotamMainCardView = itemView.FindViewById<CardView>(Resource.Id.notamCard_MainCard);
            NotamMainCardView.SetCardBackgroundColor(new ApplyTheme().GetColor(DesiredColor.CardViews));

            // Main layouts
            NotamCardMainLayout = itemView.FindViewById<LinearLayout>(Resource.Id.notamCard_MainLayout);
            NotamCardTopLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.notamCard_TopLayout);

            // Childs in TopLayout
            NotamIdTextView = itemView.FindViewById<TextView>(Resource.Id.notamCard_Id);
            NotamIdTextView.SetTextColor(new ApplyTheme().GetColor(DesiredColor.MainText));

            // Free Notam Text
            NotamFreeTextTextView = itemView.FindViewById<TextView>(Resource.Id.notamCard_FreeText);
            NotamFreeTextTextView.SetTextColor(new ApplyTheme().GetColor(DesiredColor.MainText));
            NotamFreeTextTextView.SetTextSize(ComplexUnitType.Dip, 12);
        }
    }

    internal class ErrorViewHolder : RecyclerView.ViewHolder
    {
        public TextView ErrorTextView { get; }
        public CardView ErrorCardView { get;  }

        public ErrorViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            ErrorTextView = itemView.FindViewById<TextView>(Resource.Id.notam_ErrorTextView);
            ErrorTextView.SetTextColor(new ApplyTheme().GetColor(DesiredColor.RedTextWarning));

            ErrorCardView = itemView.FindViewById<CardView>(Resource.Id.notam_ErrorMainCard);
            ErrorCardView.SetCardBackgroundColor(new ApplyTheme().GetColor(DesiredColor.LightYellowBackground));
        }
    }

    internal class CategoryViewHolder : RecyclerView.ViewHolder
    {
        public TextView CategoryTextView { get; }

        public CategoryViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            CategoryTextView = itemView.FindViewById<TextView>(Resource.Id.notam_CategoryTextView);

            CategoryTextView.SetTextColor(new ApplyTheme().GetColor(DesiredColor.CyanText));

            GradientDrawable categoryTitleBackground = new GradientDrawable();
            categoryTitleBackground.SetCornerRadius(8);
            categoryTitleBackground.SetColor(new ApplyTheme().GetColor(DesiredColor.CardViews));
            categoryTitleBackground.SetStroke(3, Color.Black);
            CategoryTextView.Background = categoryTitleBackground;
        }
    }

    internal class MyAirportRecycler
    {
        public string Name;
    }

    internal class MyNotamCardRecycler
    {
        public bool DisableTopLayout;
        public SpannableString NotamId;
        public string NotamFreeText;
    }

    internal class MyErrorRecycler
    {
        public string ErrorString;
    }

    internal class MyCategoryRecycler
    {
        public string CategoryString;
    }
}
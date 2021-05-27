using HMUI;
using IPA.Utilities;
using System;
using System.Reflection;
using UnityEngine;

namespace UIEngine
{
    public class Accessors
    {

        public static FieldAccessor<ButtonStaticAnimations, NoTransitionsButton>.Accessor ButtonStaticAnimations_button = FieldAccessor<ButtonStaticAnimations, NoTransitionsButton>.GetAccessor("_button");
        public static FieldAccessor<ButtonStaticAnimations, AnimationClip>.Accessor ButtonStaticAnimations_normalClip = FieldAccessor<ButtonStaticAnimations, AnimationClip>.GetAccessor("_normalClip");
        public static FieldAccessor<ButtonStaticAnimations, AnimationClip>.Accessor ButtonStaticAnimations_highlightedClip = FieldAccessor<ButtonStaticAnimations, AnimationClip>.GetAccessor("_highlightedClip");
        public static FieldAccessor<ButtonStaticAnimations, AnimationClip>.Accessor ButtonStaticAnimations_pressedClip = FieldAccessor<ButtonStaticAnimations, AnimationClip>.GetAccessor("_pressedClip");
        public static FieldAccessor<ButtonStaticAnimations, AnimationClip>.Accessor ButtonStaticAnimations_disabledClip = FieldAccessor<ButtonStaticAnimations, AnimationClip>.GetAccessor("_disabledClip");



        public static FieldAccessor<SelectableCellStaticAnimations, SelectableCell>.Accessor SelectableCellStaticAnimations_selectableCell = FieldAccessor<SelectableCellStaticAnimations, SelectableCell>.GetAccessor("_selectableCell");
        public static FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.Accessor SelectableCellStaticAnimations_normalAnimationClip = FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.GetAccessor("_normalAnimationClip");
        public static FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.Accessor SelectableCellStaticAnimations_highlightedAnimationClip = FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.GetAccessor("_highlightedAnimationClip");
        public static FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.Accessor SelectableCellStaticAnimations_selectedAnimationClip = FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.GetAccessor("_selectedAnimationClip");
        public static FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.Accessor SelectableCellStaticAnimations_selectedAndHighlightedAnimationClip = FieldAccessor<SelectableCellStaticAnimations, AnimationClip>.GetAccessor("_selectedAndHighlightedAnimationClip");



        public static FieldAccessor<ImageView, bool>.Accessor ImageView_gradientAccessor = FieldAccessor<ImageView, bool>.GetAccessor("_gradient");
        public static FieldAccessor<ImageView, bool>.Accessor ImageView_flipGradientColorsAccessor = FieldAccessor<ImageView, bool>.GetAccessor("_flipGradientColors");
        public static FieldAccessor<ImageView, ImageView.GradientDirection>.Accessor ImageView_gradientDirectionAccessor = FieldAccessor<ImageView, ImageView.GradientDirection>.GetAccessor("_gradientDirection");



        public static FieldAccessor<LevelListTableCell, UnityEngine.UI.Image>.Accessor LevelListTableCell_backgroundImage = FieldAccessor<LevelListTableCell, UnityEngine.UI.Image>.GetAccessor("_backgroundImage");
        public static FieldAccessor<LevelListTableCell, Color>.Accessor LevelListTableCell_selectedBackgroundColor = FieldAccessor<LevelListTableCell, Color>.GetAccessor("_selectedBackgroundColor");
        public static FieldAccessor<LevelListTableCell, Color>.Accessor LevelListTableCell_highlightBackgroundColor = FieldAccessor<LevelListTableCell, Color>.GetAccessor("_highlightBackgroundColor");
        public static FieldAccessor<LevelListTableCell, Color>.Accessor LevelListTableCell_selectedAndHighlightedBackgroundColor = FieldAccessor<LevelListTableCell, Color>.GetAccessor("_selectedAndHighlightedBackgroundColor");

    }
}

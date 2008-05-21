#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Drawing;
using MediaPortal.Presentation.Properties;
using MediaPortal.Control.InputManager;
using Presentation.SkinEngine.MarkupExtensions;

namespace Presentation.SkinEngine.Controls.Visuals
{
  public class ContentPresenter : FrameworkElement
  {
    private Property _contentProperty;
    private Property _contentTemplateProperty;
    private Property _contentTemplateSelectorProperty;
    FrameworkElement _contentCache;
    public ContentPresenter()
    {
      Init();
    }

    public ContentPresenter(ContentPresenter c)
      : base(c)
    {
      Init();
      if (c.Content != null)
      {
        Content = (FrameworkElement)c.Content.Clone();
        Content.VisualParent = this;
      }
      if (c.ContentTemplate != null)
        ContentTemplate = (DataTemplate)c.ContentTemplate.Clone();
      if (c.ContentTemplateSelector != null)
        ContentTemplateSelector = c.ContentTemplateSelector;
    }

    public override object Clone()
    {
      ContentPresenter result = new ContentPresenter(this);
      BindingMarkupExtension.CopyBindings(this, result);
      return result;
    }

    void Init()
    {
      _contentProperty = new Property(typeof(FrameworkElement), null);
      _contentTemplateProperty = new Property(typeof(DataTemplate), null);
      _contentTemplateSelectorProperty = new Property(typeof(DataTemplateSelector), null);
      _contentProperty.Attach(OnContentChanged);
    }
    void OnContentChanged(Property property)
    {
      _contentCache = _contentProperty.GetValue() as FrameworkElement;
      Content.VisualParent = this;
      Content.SetWindow(Window);
    }

    /// <summary>
    /// Gets or sets the content property.
    /// </summary>
    /// <value>The content property.</value>
    public Property ContentProperty
    {
      get
      {
        return _contentProperty;
      }
      set
      {
        _contentProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>The content.</value>
    public FrameworkElement Content
    {
      get
      {
        return _contentCache;
      }
      set
      {
        _contentProperty.SetValue(value);
      }
    }

    /// <summary>
    /// Gets or sets the content template property.
    /// </summary>
    /// <value>The content template property.</value>
    public Property ContentTemplateProperty
    {
      get
      {
        return _contentTemplateProperty;
      }
      set
      {
        _contentTemplateProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets the content template.
    /// </summary>
    /// <value>The content template.</value>
    public DataTemplate ContentTemplate
    {
      get
      {
        return _contentTemplateProperty.GetValue() as DataTemplate;
      }
      set
      {
        _contentTemplateProperty.SetValue(value);
      }
    }


    /// <summary>
    /// Gets or sets the content template selector property.
    /// </summary>
    /// <value>The content template selector property.</value>
    public Property ContentTemplateSelectorProperty
    {
      get
      {
        return _contentTemplateSelectorProperty;
      }
      set
      {
        _contentTemplateSelectorProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets the content template selector.
    /// </summary>
    /// <value>The content template selector.</value>
    public DataTemplateSelector ContentTemplateSelector
    {
      get
      {
        return _contentTemplateSelectorProperty.GetValue() as DataTemplateSelector;
      }
      set
      {
        _contentTemplateSelectorProperty.SetValue(value);
      }
    }

    /// <summary>
    /// measures the size in layout required for child elements and determines a size for the FrameworkElement-derived class.
    /// </summary>
    /// <param name="availableSize">The available size that this element can give to child elements.</param>
    public override void Measure(SizeF availableSize)
    {
      float marginWidth = (float)((Margin.X + Margin.W) * SkinContext.Zoom.Width);
      float marginHeight = (float)((Margin.Y + Margin.Z) * SkinContext.Zoom.Height);
      _desiredSize = new System.Drawing.SizeF((float)Width * SkinContext.Zoom.Width, (float)Height * SkinContext.Zoom.Height);
      
      // Width / Height is not set.
      if (Width == 0)
        _desiredSize.Width = (float)(availableSize.Width - marginWidth);
      if (Height == 0)
        _desiredSize.Height = (float)(availableSize.Height - marginHeight);

      if (LayoutTransform != null)
      {
        ExtendedMatrix m;
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }

      // Calculate how much is available for the child
      SizeF childSize = new SizeF(_desiredSize.Width, _desiredSize.Height);

      // It can not be less than zero in any dimension
      if (childSize.Width < 0) 
        childSize.Width = 0;
      if (childSize.Height < 0) 
        childSize.Height = 0;

      // Do we have a child
      if (Content != null)
      {
        // Measure the child
        Content.Measure(childSize);

        // Is the desired size of the child bigger than our self?
        // Then we must expand our desire.
        if (Content.DesiredSize.Height > _desiredSize.Height)
          _desiredSize.Height = Content.DesiredSize.Height;

        if (Content.DesiredSize.Width > _desiredSize.Width)
          _desiredSize.Width = Content.DesiredSize.Width;
      }

      // Do we have a fixed Width / Height?
      if (Width > 0) 
        _desiredSize.Width = (float)Width * SkinContext.Zoom.Width;
      if (Height > 0) 
        _desiredSize.Height = (float)Height * SkinContext.Zoom.Height;

      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }

      SkinContext.FinalLayoutTransform.TransformSize(ref _desiredSize);
      _desiredSize.Width += (float)marginWidth;
      _desiredSize.Height += (float)marginHeight;
      _originalSize = _desiredSize;



      _availableSize = new SizeF(availableSize.Width, availableSize.Height);
    }

    /// <summary>
    /// Arranges the UI element
    /// and positions it in the finalrect
    /// </summary>
    /// <param name="finalRect">The final size that the parent computes for the child element</param>
    public override void Arrange(System.Drawing.RectangleF finalRect)
    {
      _finalRect = new System.Drawing.RectangleF(finalRect.Location, finalRect.Size);
      System.Drawing.RectangleF layoutRect = new System.Drawing.RectangleF(finalRect.X, finalRect.Y, finalRect.Width, finalRect.Height);
      layoutRect.X += (float)(Margin.X * SkinContext.Zoom.Width);
      layoutRect.Y += (float)(Margin.Y * SkinContext.Zoom.Height);
      layoutRect.Width -= (float)((Margin.X + Margin.W) * SkinContext.Zoom.Width);
      layoutRect.Height -= (float)((Margin.Y + Margin.Z) * SkinContext.Zoom.Height);
      ActualPosition = new SlimDX.Vector3(layoutRect.Location.X, layoutRect.Location.Y, 1.0f); ;
      ActualWidth = layoutRect.Width;
      ActualHeight = layoutRect.Height;
      if (LayoutTransform != null)
      {
        ExtendedMatrix m = new ExtendedMatrix();
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }

      if (Content != null)
      {
        PointF location = new PointF((float)(layoutRect.X), (float)(layoutRect.Y));
        SizeF size = new SizeF(Content.DesiredSize.Width, Content.DesiredSize.Height);
        ArrangeChild(Content, ref location, (double)layoutRect.Width, (double)layoutRect.Height);
        Content.Arrange(new RectangleF(location, size));
      }

      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }
      _finalLayoutTransform = SkinContext.FinalLayoutTransform;

      IsArrangeValid = true;
      Initialize();
      InitializeTriggers();
      _isLayoutInvalid = false;
    }

    protected void ArrangeChild(FrameworkElement child, ref System.Drawing.PointF p, double widthPerCell, double heightPerCell)
    {
      if (VisualParent == null) return;

      if (child.HorizontalAlignment == HorizontalAlignmentEnum.Center)
      {
        p.X += (float)((widthPerCell - child.DesiredSize.Width) / 2);
      }
      else if (child.HorizontalAlignment == HorizontalAlignmentEnum.Right)
      {
        p.X += (float)(widthPerCell - child.DesiredSize.Width);
      }
      if (child.VerticalAlignment == VerticalAlignmentEnum.Center)
      {
        p.Y += (float)((heightPerCell - child.DesiredSize.Height) / 2);
      }
      else if (child.VerticalAlignment == VerticalAlignmentEnum.Bottom)
      {
        p.Y += (float)(heightPerCell - child.DesiredSize.Height);
      }
    }
    /// <summary>
    /// Renders the visual
    /// </summary>
    public override void DoRender()
    {
      base.DoRender();
      if (Content != null)
      {
        SkinContext.AddOpacity(this.Opacity);
        Content.DoRender();
        SkinContext.RemoveOpacity();
      }
    }

    public override void DoBuildRenderTree()
    {
      if (!IsVisible) return;
      if (Content != null)
      {
        Content.BuildRenderTree();
      }
    }
    public override void DestroyRenderTree()
    {
      if (Content != null)
      {
        Content.DestroyRenderTree();
      }
    }

    /// <summary>
    /// Called when [mouse move].
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public override void OnMouseMove(float x, float y)
    {
      if (!IsFocusScope) return;
      if (Content != null)
      {
        Content.OnMouseMove(x, y);
      }
      base.OnMouseMove(x, y);
    }


    public override void FireUIEvent(UIEvent eventType, UIElement source)
    {
      if (Content != null)
        Content.FireUIEvent(eventType,  source);
    }
    /// <summary>
    /// Handles keypresses
    /// </summary>
    /// <param name="key">The key.</param>
    public override void OnKeyPressed(ref MediaPortal.Control.InputManager.Key key)
    {
      if (Content != null)
      {
        Content.OnKeyPressed(ref key);
      }
    }

    /// <summary>
    /// Find the element with name
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public override UIElement FindElement(string name)
    {
      if (Content != null)
      {
        UIElement found = Content.FindElement(name);
        if (found != null) return found;
      }
      return base.FindElement(name);
    }

    /// <summary>
    /// Finds the element of type t.
    /// </summary>
    /// <param name="t">The t.</param>
    /// <returns></returns>
    public override UIElement FindElementType(Type t)
    {
      if (Content != null)
      {
        UIElement found = Content.FindElementType(t);
        if (found != null) return found;
      }
      return base.FindElementType(t);
    }

    /// <summary>
    /// Finds the the element which is a ItemsHost
    /// </summary>
    /// <returns></returns>
    public override UIElement FindItemsHost()
    {
      if (Content != null)
      {
        UIElement found = Content.FindItemsHost();
        if (found != null) return found;
      }
      return base.FindItemsHost();
    }

    /// <summary>
    /// Finds the focused item.
    /// </summary>
    /// <returns></returns>
    public override UIElement FindFocusedItem()
    {
      if (HasFocus) return this;
      if (Content != null)
      {
        UIElement found = Content.FindFocusedItem();
        return found;
      }
      return null;
    }
    public override void Reset()
    {
      base.Reset();
      if (Content != null)
        Content.Reset();
    }
    public override void Deallocate()
    {
      base.Deallocate();
      if (Content != null)
      {
        Content.Deallocate();
      }
    }
    public override void Allocate()
    {
      base.Allocate();
      if (Content != null)
      {
        Content.Allocate();
      }
    }
    #region focus prediction

    /// <summary>
    /// Predicts the next FrameworkElement which is position above this FrameworkElement
    /// </summary>
    /// <param name="focusedFrameworkElement">The current  focused FrameworkElement.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public override FrameworkElement PredictFocusUp(FrameworkElement focusedFrameworkElement, ref Key key, bool strict)
    {
      return ((FrameworkElement)Content).PredictFocusUp(focusedFrameworkElement, ref key, strict);
    }

    /// <summary>
    /// Predicts the next FrameworkElement which is position below this FrameworkElement
    /// </summary>
    /// <param name="focusedFrameworkElement">The current  focused FrameworkElement.</param>
    /// <param name="key">The MediaPortal.Control.InputManager.Key.</param>
    /// <returns></returns>
    public override FrameworkElement PredictFocusDown(FrameworkElement focusedFrameworkElement, ref Key key, bool strict)
    {
      return ((FrameworkElement)Content).PredictFocusDown(focusedFrameworkElement, ref key, strict);
    }

    /// <summary>
    /// Predicts the next FrameworkElement which is position left of this FrameworkElement
    /// </summary>
    /// <param name="focusedFrameworkElement">The current  focused FrameworkElement.</param>
    /// <param name="key">The MediaPortal.Control.InputManager.Key.</param>
    /// <returns></returns>
    public override FrameworkElement PredictFocusLeft(FrameworkElement focusedFrameworkElement, ref Key key, bool strict)
    {
      return ((FrameworkElement)Content).PredictFocusLeft(focusedFrameworkElement, ref key, strict);
    }

    /// <summary>
    /// Predicts the next FrameworkElement which is position right of this FrameworkElement
    /// </summary>
    /// <param name="focusedFrameworkElement">The current  focused FrameworkElement.</param>
    /// <param name="key">The MediaPortal.Control.InputManager.Key.</param>
    /// <returns></returns>
    public override FrameworkElement PredictFocusRight(FrameworkElement focusedFrameworkElement, ref Key key, bool strict)
    {
      return ((FrameworkElement)Content).PredictFocusRight(focusedFrameworkElement, ref key, strict);
    }


    #endregion
    public override void SetWindow(Window window)
    {
      base.SetWindow(window);
      if (Content != null)
      {
        Content.SetWindow(window);
      }
    }
  }
}

<?xml version="1.0" encoding="utf-8"?>
<Grid
  xmlns="www.team-mediaportal.com/2008/mpf/directx"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:fanart="clr-namespace:MediaPortal.Extensions.UserServices.FanArtService.Client;assembly=FanArtService.Client"
  xmlns:media="clr-namespace:MediaPortal.UiComponents.Media.Models.AspectWrappers;assembly=Media"
  Grid.Column="2" Grid.Row="0" x:Name="MediaItemDetails"
  HorizontalAlignment="Stretch" VerticalAlignment="Stretch">

  <Grid.RowDefinitions>
    <RowDefinition Height="*"/>
  </Grid.RowDefinitions>
  <Grid.ColumnDefinitions>
    <ColumnDefinition Width="380"/>
    <ColumnDefinition Width="*"/>
  </Grid.ColumnDefinitions>

  <StackPanel Grid.Column="1" Margin="10" HorizontalAlignment="Stretch" VerticalAlignment="Stretch">
    <media:MediaAspectWrapper MediaItem="{Binding Source={StaticResource FanArtBackgroundModel},Path=MediaItem}">
      <media:MediaAspectWrapper.Template>
        <ControlTemplate>
          <Grid DataContext="{Binding RelativeSource={RelativeSource TemplatedParent}}">
            <Grid.RowDefinitions>
              <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
              <ColumnDefinition Width="180"/>
              <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>

            <Label x:Name="YearLabel" Grid.Row="0" Grid.Column="0"
                Content="[Media.VideoYearLabel]" Color="{ThemeResource EmphasisTextColor}"/>
            <Label x:Name="Year" Grid.Row="0" Grid.Column="1"
                Content="{Binding Path=RecordingTime, Converter={StaticResource DateFormatConverter}, ConverterParameter=yyyy}" Color="{ThemeResource TextColor}"/>

          </Grid>
        </ControlTemplate>
      </media:MediaAspectWrapper.Template>
    </media:MediaAspectWrapper>

    <media:VideoAspectWrapper MediaItem="{Binding Source={StaticResource FanArtBackgroundModel},Path=MediaItem}">
      <media:VideoAspectWrapper.Template>
        <ControlTemplate>
          <Grid DataContext="{Binding RelativeSource={RelativeSource TemplatedParent}}">
            <Grid.RowDefinitions>
              <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
              <ColumnDefinition Width="180"/>
              <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>

            <Label x:Name="GenreLabel" Grid.Row="0" Grid.Column="0"
                Content="[Media.VideoGenreLabel]" Color="{ThemeResource EmphasisTextColor}"/>
            <Label x:Name="Genre" Grid.Row="0" Grid.Column="1"
                Content="{Binding Genres}" Color="{ThemeResource TextColor}"/>

          </Grid>
        </ControlTemplate>
      </media:VideoAspectWrapper.Template>
    </media:VideoAspectWrapper>

    <media:MovieAspectWrapper MediaItem="{Binding Source={StaticResource FanArtBackgroundModel},Path=MediaItem}">
      <media:MovieAspectWrapper.Template>
        <ControlTemplate>
          <Grid DataContext="{Binding RelativeSource={RelativeSource TemplatedParent}}">
            <Grid.RowDefinitions>
              <RowDefinition Height="Auto"/>
              <RowDefinition Height="Auto"/>
              <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
              <ColumnDefinition Width="180"/>
              <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>

            <Label x:Name="MovieLabel" Grid.Row="0" Grid.Column="0"
                Content="[Media.VideoTitleLabel]" Color="{ThemeResource EmphasisTextColor}"/>
            <Label x:Name="MovieName" Grid.Row="0" Grid.Column="1"
                Content="{Binding MovieName}" Color="{ThemeResource TextColor}"/>

            <Label x:Name="CollectionLabel" Grid.Row="1" Grid.Column="0"
                Content="[Media.VideoCollectionLabel]" Color="{ThemeResource EmphasisTextColor}"/>
            <Label x:Name="Collection" Grid.Row="1" Grid.Column="1"
                Content="{Binding CollectionName}" Color="{ThemeResource TextColor}"/>

            <Label x:Name="DurationLabel" Grid.Row="2" Grid.Column="0"
                Content="[Media.VideoDurationLabel]" Color="{ThemeResource EmphasisTextColor}"/>
            <Label x:Name="Duration" Grid.Row="2" Grid.Column="1"
                Content="{Binding Path=Runtime, Converter={StaticResource DurationConverter}, ConverterParameter=hh\:mm}" Color="{ThemeResource TextColor}"/>

          </Grid>
        </ControlTemplate>
      </media:MovieAspectWrapper.Template>
    </media:MovieAspectWrapper>

    <Label x:Name="StoryPlot" Wrap="True" Scroll="Auto" ScrollSpeed="5" Margin="0,10,0,0" Height="320"
        HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
        Content="{Binding Source={StaticResource FanArtBackgroundModel}, Path=ItemDescription}"
        Color="{ThemeResource TextColor}" FontSize="{ThemeResource SmallFontSize}"/>
  </StackPanel>


  <Image Grid.Column="0" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" Stretch="Uniform" Margin="5" 
      Width="380" Height="560">
    <Image.Source>
      <fanart:FanArtImageSource 
        fanart:FanArtMediaType="{Binding Source={StaticResource FanArtBackgroundModel}, Path=FanArtMediaType}" 
        fanart:FanArtType="Poster" 
        fanart:FanArtName="{Binding Source={StaticResource FanArtBackgroundModel}, Path=FanArtName}"
        fanart:MaxWidth="380" fanart:MaxHeight="560"/>
    </Image.Source>
  </Image>
</Grid>

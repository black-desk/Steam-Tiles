﻿Imports FontAwesome.UWP
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Tiles"), FontAwesomeIcon.Home, 0))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesomeIcon.Cog, 1))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If Not item Is Nothing Then
            If item.Text = recursos.GetString("Tiles") Then
                GridVisibilidad(gridTiles, item.Text)

                If spProgreso.Visibility = Visibility.Visible Then
                    gridSeleccionarJuego.Visibility = Visibility.Collapsed
                End If

                If gridAvisoNoJuegos.Visibility = Visibility.Visible Then
                    gridSeleccionarJuego.Visibility = Visibility.Collapsed
                End If

                If Not ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles") = 0 Then
                    gvTiles.Width = ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles")
                    gvTiles.Padding = New Thickness(5, 0, 5, 0)
                End If

            ElseIf item.Text = recursos.GetString("Config") Then
                GridVisibilidad(gridConfig, item.Text)
            End If
        End If

    End Sub

    Private Sub Nv_ItemFlyout(sender As NavigationViewItem, args As TappedRoutedEventArgs)

        FlyoutBase.ShowAttachedFlyout(sender)

    End Sub

    Private Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        MasCosas.Generar()

        nvPrincipal.IsPaneOpen = False

        Configuracion.Iniciar()

        '--------------------------------------------------------

        Dim transpariencia As New UISettings
        TransparienciaEfectosFinal(transpariencia.AdvancedEffectsEnabled)
        AddHandler transpariencia.AdvancedEffectsEnabledChanged, AddressOf TransparienciaEfectosCambia

    End Sub

    Private Sub TransparienciaEfectosCambia(sender As UISettings, e As Object)

        TransparienciaEfectosFinal(sender.AdvancedEffectsEnabled)

    End Sub

    Private Async Sub TransparienciaEfectosFinal(estado As Boolean)

        Await Dispatcher.RunAsync(CoreDispatcherPriority.High, Sub()
                                                                   If estado = True Then
                                                                       gridPersonalizarTiles.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfig.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfigTiles.Background = App.Current.Resources("GridTituloBackground")
                                                                   Else
                                                                       gridPersonalizarTiles.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfigTiles.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                   End If
                                                               End Sub)

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        gridAñadirTile.Visibility = Visibility.Collapsed
        gridPersonalizarTiles.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    'TILES-----------------------------------------------------------------------------

    Private Sub BotonAñadirTile_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirTile.Click

        Dim tile As Tile = botonAñadirTile.Tag
        Tiles.Añadir.Generar(tile)

    End Sub

    Private Sub BotonCerrarTiles_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarTiles.Click

        gridAñadirTile.Visibility = Visibility.Collapsed
        gridSeleccionarJuego.Visibility = Visibility.Visible

        If ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles") > 0 Then
            If Steam.anchoColumna < ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles") Then
                gvTiles.Width = ApplicationData.Current.LocalSettings.Values("ancho_grid_tiles")
                gvTiles.Padding = New Thickness(5, 0, 5, 0)
            End If
        End If

    End Sub

    Private Sub BotonTilePequeña_Click(sender As Object, e As RoutedEventArgs) Handles botonTilePequeña.Click

        Tiles.Personalizacion.Cargar(gridTilePequeña, 0, imagenTilePequeña.Source)

    End Sub

    Private Sub BotonTileMediana_Click(sender As Object, e As RoutedEventArgs) Handles botonTileMediana.Click

        Tiles.Personalizacion.Cargar(gridTileMediana, 1, imagenTileMediana.Source)

    End Sub

    Private Sub BotonTileAncha_Click(sender As Object, e As RoutedEventArgs) Handles botonTileAncha.Click

        Tiles.Personalizacion.Cargar(gridTileAncha, 2, imagenTileAncha.Source)

    End Sub

    Private Sub BotonTileGrande_Click(sender As Object, e As RoutedEventArgs) Handles botonTileGrande.Click

        Tiles.Personalizacion.Cargar(gridTileGrande, 3, imagenTileGrande.Source)

    End Sub

    'CONFIG-----------------------------------------------------------------------------

    Private Sub CbConfigModosTiles_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbConfigModosTiles.SelectionChanged

        Configuracion.ModoTiles(cbConfigModosTiles.SelectedIndex, False)

    End Sub

    Private Sub BotonAñadirCarpetaSteam_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirCarpetaSteam.Click

        Steam.Generar(True)

    End Sub

    Private Sub BotonBorrarCarpetasSteam_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarCarpetasSteam.Click

        Steam.Borrar()

    End Sub

    Private Sub TbConfigCuenta_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbConfigCuenta.TextChanged

        If tbConfigCuenta.Text.Trim.Length > 0 Then
            Steam.Cuenta(tbConfigCuenta.Text.Trim)
        End If

    End Sub

    Private Sub BotonConfigLimpiarCache_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigLimpiarCache.Click

        Cache.Limpiar()

    End Sub

End Class

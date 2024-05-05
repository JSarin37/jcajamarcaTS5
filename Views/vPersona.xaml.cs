using jcajamarcaTS5.Modelos;

namespace jcajamarcaTS5.Views;

public partial class vPersona : ContentPage
{

    public vPersona()
    {
        InitializeComponent();
    }

    private Persona selectedPersona; // Variable para almacenar el contacto seleccionado para editar
    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        if (selectedPersona == null)
        {
            Persona newPersona = new Persona
            {
                Name = txtPersona.Text,

            };
            lblStatus.Text = "";
            App.PersonRepo.AddNewPerson(newPersona);
            lblStatus.Text = App.PersonRepo.statusMessage;
            // Actualizar la lista de personas después de la eliminación
            ActualizarListaDePersonas();
        }
        else
        {
            selectedPersona.Name = txtPersona.Text;
            lblStatus.Text = "";
            App.PersonRepo.AddNewPerson(selectedPersona);
            lblStatus.Text = App.PersonRepo.statusMessage;
            selectedPersona = null; // Resetea el contacto seleccionado después de la actualización
                                    // Actualizar la lista de personas después de la eliminación
            ActualizarListaDePersonas();

        }
        ClearFields();

    }

    private void btnObtener_Clicked(object sender, EventArgs e)
    {
        lblStatus.Text = "";
        List<Persona> people = App.PersonRepo.GetAllPeople();
        Listapersonas.ItemsSource = people;
    }

    private void btnEditar_Clicked(object sender, EventArgs e)
    {
        selectedPersona = (sender as Button).CommandParameter as Persona;
        txtPersona.Text = selectedPersona.Name;

    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        // Obtener la persona seleccionada en el CollectionView
        selectedPersona = (sender as Button).CommandParameter as Persona;
        txtPersona.Text = selectedPersona.Name;

        if (selectedPersona != null)
        {
            bool confirmarEliminar = await MostrarConfirmacion("¿Estás seguro de que deseas eliminar a " + selectedPersona.Name + "?");

            if (confirmarEliminar)
            {
                int personIdToDelete = selectedPersona.Id;
                App.PersonRepo.DeletePerson(personIdToDelete); // Llamar al método DeletePerson del repositorio de personas
                lblStatus.Text = App.PersonRepo.statusMessage;
                // Actualizar la lista de personas después de la eliminación
                ActualizarListaDePersonas();

            }
            ClearFields();
        }
        else
        {
            // Manejar el caso en que no se ha seleccionado ninguna persona
            await MostrarMensajeError("Debes seleccionar una persona de la lista");
        }
    }

    private int ObtenerIdPersonaAEliminar()
    {
        // Obtener la persona seleccionada en el CollectionView
        Persona selectedPerson = Listapersonas.SelectedItem as Persona;

        if (selectedPerson != null)
        {
            return selectedPerson.Id;
        }
        else
        {
            // Manejar el caso en que no se ha seleccionado ninguna persona
            MostrarMensajeError("Debes seleccionar una persona de la lista");
            return -1; // Valor predeterminado o indicador de que no se seleccionó ninguna persona
        }
    }

    private void ClearFields()
    {
        txtPersona.Text = string.Empty;

    }
    private async Task<bool> MostrarConfirmacion(string mensaje)
    {
        return await App.Current.MainPage.DisplayAlert("Confirmar", mensaje, "Sí", "Cancelar");
    }

    private async Task MostrarMensajeError(string mensaje)
    {
        await App.Current.MainPage.DisplayAlert("Error", mensaje, "Aceptar");
    }

    private void ActualizarListaDePersonas()
    {
        // Recargar los datos de la lista de personas
        Listapersonas.ItemsSource = null;
        Listapersonas.ItemsSource = ObtenerListaDePersonas(); // Debes implementar este método para obtener la lista actualizada de personas
    }

    // Método para obtener la lista actualizada de personas desde tu repositorio de datos
    private List<Persona> ObtenerListaDePersonas()
    {
        return App.PersonRepo.GetAllPeople();
    }
}
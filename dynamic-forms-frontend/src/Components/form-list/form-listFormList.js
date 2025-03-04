import React, { useEffect, useState } from 'react';
import axios from 'axios';
import FormularioModal from '../form-edit/form-EditarOAgregarModal';
import DeleteModal from '../form-delete/form-DeleteModal';

function FormList() {
  const [formularios, setFormularios] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isModalDeleteOpen, setIsModaldeleteOpen] = useState(false);
  const [id_selecionado, setIdSelecionado] = useState(0);
  const [nombreSlecionado, setNombreSelecionado] = useState(0);

  useEffect(() => {
    axios
      .get('http://localhost:5033/api/Formulario/formularios')
      .then((response) => {
        setFormularios(response.data);
        console.log(response.data)
      })
      .catch((error) => {
        console.error('Hubo un error al cargar los formularios:', error);
      });
  }, []);
  
  const handleOpenModal = () => {setIsModalOpen(true); };
  const handleCloseModal = () => {setIsModalOpen(false); };
  const reiniciarID = ()=> {setIdSelecionado(0); };
  const handleOpenDeleteModal = () => {setIsModaldeleteOpen(true); };
  const handleCloseDeleteModal = () => {setIsModaldeleteOpen(false); };

  const fetchFormularios = () => {
    axios
      .get('http://localhost:5033/api/Formulario/formularios')
      .then((response) => {
        setFormularios(response.data);
        console.log(response.data);
      })
      .catch((error) => {
        console.error('Hubo un error al cargar los formularios:', error);
      });
  };

  return (
    <div className="container mt-5">
      <div className='d-flex justify-content-between align-items-center mb-4'>
        <h1 className="text-center ">Lista de Formularios</h1> 
        <button onClick={() => {reiniciarID(); handleOpenModal();  }} className='btn btn-secondary h-50'  >Agregar Formulario</button>
      </div>
      <FormularioModal
        isOpen={isModalOpen}     
        onClose={handleCloseModal}
        fetchFormularios={fetchFormularios} 
        formularioId={id_selecionado}
      />
      <DeleteModal isOpen={isModalDeleteOpen} onClose={handleCloseDeleteModal} id={id_selecionado} 
      nombreForm={nombreSlecionado} fetchFormularios={fetchFormularios} 
      />
      {/* Si no hay formularios disponibles */}
      {formularios.length === 0 ? (
       
        
        <div className="alert alert-warning text-center" role="alert">
          No hay formularios disponibles.
        </div>
        
      ) : (
        <div className="row">
          {/* Itera sobre los formularios y muestra cada uno */}
          {formularios.map((formulario) => (
            <div key={formulario.id} id={formulario.id} className="col-md-4 mb-4 w-50">
              <div className="card " style={{boxShadow: 'rgba(101, 119, 134, 0.2) 0px 0px 15px, rgba(101, 119, 134, 0.15) 0px 0px 3px 1px',backgroundColor: '#f8f9fa', padding: '20px', borderRadius: '5px'}}>
                <div className="card-body p-0">
                  <h5 className="card-title">{formulario.nombre}</h5>
                  <span>Columnas: </span>
                  {formulario.campos && formulario.campos.length > 0 ? (
                    formulario.campos.map((campo, index) => (
                      <span key={index}>
                        {campo.nombre}
                        {index < formulario.campos.length - 1 && ', '}
                      </span>
                    ))
                  ) : (
                    <span>No hay columnas disponibles</span>
                  )}

                  <br></br>
                  <div className='d-flex gap-2 mt-2 justify-content-center flex-column flex-sm-row'>
                    <button
                      onClick={() => {
                        setIdSelecionado(formulario.id);
                        setNombreSelecionado(formulario.nombre);
                        handleOpenModal();
                      }} 
                      className="btn btn-primary "
                    >
                      Editar
                    </button>

                    <button
                      onClick={() => {
                        // Aquí va el código para mostrar la vista previa, por ejemplo:
                        setIdSelecionado(formulario.id);
                        setNombreSelecionado(formulario.nombre);
                          // Implementa la función `handleOpenPreviewModal` para mostrar la vista previa
                      }} 
                      className="btn btn-warning ">
                      Vista previa
                    </button>

                    <button
                    onClick={() => {
                      setIdSelecionado(formulario.id);
                      setNombreSelecionado(formulario.nombre);
                      handleOpenDeleteModal();
                    }} 
                    className="btn btn-danger ">
                    Eliminar 
                    </button>

                    


                  </div>
                  
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
      
    </div>
  );


}

export default FormList;

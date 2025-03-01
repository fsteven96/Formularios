import React, { useEffect, useState } from 'react';
import axios from 'axios';

function FormList() {
  const [formularios, setFormularios] = useState([]);

  useEffect(() => {
    axios
      .get('http://localhost:5033/api/Formulario')
      .then((response) => {
        setFormularios(response.data);
        console.log(response.data)
      })
      .catch((error) => {
        console.error('Hubo un error al cargar los formularios:', error);
      });
  }, []);

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Lista de Formularios</h1>
      
      {/* Si no hay formularios disponibles */}
      {formularios.length === 0 ? (
        <div className="alert alert-warning text-center" role="alert">
          No hay formularios disponibles.
        </div>
      ) : (
        <div className="row">
          {/* Itera sobre los formularios y muestra cada uno */}
          {formularios.map((formulario) => (
            <div key={formulario.id} className="col-md-4 mb-4">
              <div className="card shadow-sm" style={{backgroundColor: '#f8f9fa', padding: '20px', borderRadius: '5px'}}>
                <div className="card-body">
                  <h5 className="card-title">{formulario.nombre}</h5>
                  <p className="card-text">
                    Administra los campos de este formulario o crea nuevos.
                  </p>
                  {/* {<button
                    onClick={() => (window.location.href = `/formulario/${formulario.id}`)}
                    className="btn btn-primary w-100"
                  >
                    Editar/Crear Campos
                  </button>} */}
                  <div className='d-flex gap-2'>
                    <button
                      onClick={() => (window.location.href = `/formulario/${formulario.id}`)}
                      className="btn btn-primary w-50"
                    >
                      Agregar
                    </button>
                    <button
                      onClick={() => (window.location.href = `/formulario/${formulario.id}`)}
                      className="btn btn-warning w-50"
                    >
                      Visualizar
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

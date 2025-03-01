import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

function FormularioForm() {
  const [formulario, setFormulario] = useState(null);
  const [nombreCampo, setNombreCampo] = useState('');
  const [tipoCampo, setTipoCampo] = useState('');
  const { id } = useParams(); // Obtenemos el ID del formulario desde la URL

  useEffect(() => {
    // Cargar el formulario y sus campos por ID
    axios
      .get(`http://localhost:5000/api/formulario/${id}`)
      .then((response) => {
        setFormulario(response.data);
      })
      .catch((error) => {
        console.error('Hubo un error al cargar el formulario:', error);
      });
  }, [id]);

  const agregarCampo = () => {
    if (nombreCampo && tipoCampo) {
      const nuevoCampo = {
        nombre: nombreCampo,
        tipo: tipoCampo,
      };

      // Lógica para agregar el campo al backend
      axios
        .post(`http://localhost:5000/api/formulario/${id}/campo`, nuevoCampo)
        .then((response) => {
          setFormulario({
            ...formulario,
            campos: [...formulario.campos, response.data],
          });
          setNombreCampo('');
          setTipoCampo('');
        })
        .catch((error) => {
          console.error('Error al agregar el campo:', error);
        });
    }
  };

  return (
    <div>
      {formulario ? (
        <>
          <h1>Formulario: {formulario.nombre}</h1>
          <h2>Campos</h2>
          <ul>
            {formulario.campos.map((campo) => (
              <li key={campo.id}>
                {campo.nombre} ({campo.tipo})
              </li>
            ))}
          </ul>

          <h3>Agregar Nuevo Campo</h3>
          <input
            type="text"
            placeholder="Nombre del Campo"
            value={nombreCampo}
            onChange={(e) => setNombreCampo(e.target.value)}
          />
          <input
            type="text"
            placeholder="Tipo del Campo"
            value={tipoCampo}
            onChange={(e) => setTipoCampo(e.target.value)}
          />
          <button onClick={agregarCampo}>Agregar Campo</button>
        </>
      ) : (
        <p>Cargando...</p>
      )}
    </div>
  );
}

export default FormularioForm;

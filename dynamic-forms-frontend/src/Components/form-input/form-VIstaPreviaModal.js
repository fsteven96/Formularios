import React, { useState, useEffect } from "react";
import "./form-VIstaPreviaModal.css";
import axios from "axios";

function VistaPreviaModal({ isOpen, onClose, formularioId, fetchFormularios }) {
  const [formulario, setFormulario] = useState(null);
  const [respuestas, setRespuestas] = useState({});
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!isOpen) return;

    const fetchFormulario = async () => {
      try {
        const response = await axios.get(`http://localhost:5033/api/formulario/${formularioId}`);
        console.log(response.data)
        setFormulario(response.data);  
        setError(null);
      } catch (err) {
        setError("Hubo un error al cargar el formulario");
      }
    };

    fetchFormulario();
  }, [isOpen, formularioId]);

  const handleInputChange = (e, campoId) => {
    const value = e.target.type === "checkbox" ? e.target.checked : e.target.value;
    setRespuestas(prev => ({
      ...prev,
      [campoId]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const respuestasToSend = Object.keys(respuestas).map(campoId => ({
      FormularioId: formulario.id,  
      CampoId: parseInt(campoId),   
      Valor: respuestas[campoId],   
    }));

    console.log(respuestasToSend)
    axios.post("http://localhost:5033/api/respuesta", respuestasToSend, {
      headers: {
        "Content-Type": "application/json", 
      },
    })
      .then(response => {
        alert("Respuestas guardadas con éxito:");
        onClose(); 
        fetchFormularios()
      })
      .catch(error => {
        alert("Error al guardar");
        console.log("Error al guardar respuestas:", error);
      });
    
  };

  if (!isOpen || !formulario) return null;

  return (
    <>
      <div className="modal-backdrop-custom" onClick={onClose}></div>

      <div className="modal d-block" tabIndex="-1">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Vista Previa del Formulario</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>

            <div className="modal-body">
              {error && <div className="alert alert-danger">{error}</div>}
              <h4>Formulario de {formulario.nombre}</h4>
              <form onSubmit={handleSubmit}>
                {formulario.campos.map(campo => (
                  <div key={campo.id} className="mb-3">
                    {campo.tipo == "texto" && (
                      <><label htmlFor={`campo-${campo.id}`} className="form-label">{campo.nombre}</label>
                      <input
                        type="text"
                        required
                        className="form-control"
                        id={`campo-${campo.id}`}
                        onChange={(e) => handleInputChange(e, campo.id)}
                      />
                      </>
                    )}
                    {campo.tipo == "numero" && (
                      <><label htmlFor={`campo-${campo.id}`} className="form-label">{campo.nombre}</label>
                      <input
                        type="number"
                        required
                        className="form-control"
                        id={`campo-${campo.id}`}
                        onChange={(e) => handleInputChange(e, campo.id)}
                      />
                      </>
                    )}
                    {campo.tipo == "fecha" && (
                      <><label htmlFor={`campo-${campo.id}`} className="form-label">{campo.nombre}</label>
                      <input
                        type="date"
                        className="form-control"
                        id={`campo-${campo.id}`}
                        required
                        onChange={(e) => handleInputChange(e, campo.id)}
                       
                      />
                      </>
                    )}
                    {campo.tipo == "booleano" && (
                      <div className="form-check d-flex align-items-center">
                      <input 
                        type="checkbox"
                        className="form-check-input me-2" // Añadimos un margen a la derecha para separación mínima
                        id={`campo-${campo.id}`}
                        onChange={(e) => handleInputChange(e, campo.id)}
                      />
                      <label className="form-check-label" htmlFor={`campo-${campo.id}`}>
                        {campo.nombre}
                      </label>
                    </div>
                    
                    
                    )}
                  </div>
                ))}
                <div className="modal-footer">
                  <button onClick={onClose} className="btn btn-secondary">
                    Cancelar
                  </button>
                  <button type="submit" className="btn btn-primary">
                    Guardar
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
export default VistaPreviaModal;

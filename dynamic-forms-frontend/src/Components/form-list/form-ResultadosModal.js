import React, { useState, useEffect } from "react";
import "./form-ResultadosModal.css";
import axios from "axios";

function ResultadosModal({ isOpen, onClose, formularioId, fetchFormularios }) {
  const [respuestas, setRespuestas] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    
    if (isOpen && formularioId) {
      axios
        .get(`http://localhost:5033/api/respuesta/${formularioId}`) 
        .then((response) => {
          console.log(response.data)
          setRespuestas(response.data);
          setError(null); 
        })
        .catch((error) => {
          setError("Hubo un problema al cargar los resultados.");
          setRespuestas([]);
        });
    }
  }, [isOpen, formularioId]); 
  if (!isOpen) return null;

  const transformedData = respuestas.reduce((acc, respuesta) => {
    const { campoNombre, valor } = respuesta;
    if (!acc[campoNombre]) {
      acc[campoNombre] = [];
    }
    acc[campoNombre].push(valor);
    return acc;
  }, {});


  const columnas = Object.keys(transformedData);

  return (
    <>
      <div className="modal-backdrop-custom" onClick={onClose}></div>
      <div className="modal d-block" tabIndex="-1">
      <div className="modal-dialog modal-dialog-scrollable" style={{maxwidth: '100%'}}>
          <div className="modal-content" >
            <div className="modal-header">
              <h5 className="modal-title">Resultados</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>

            <div className="modal-body">
              {error && <div className="alert alert-danger">{error}</div>}

              <table className="table">
                <thead>
                  <tr>
                    <th scope="col">Indice</th>
                    {columnas.map((columna, index) => (
                      <th scope="col" key={index}>{columna}</th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  {transformedData[columnas[0]]?.map((_, index) => (
                    <tr key={index}>
                      <td>{index + 1}</td>
                      {columnas.map((columna, idx) => (
                        <td key={idx}>{transformedData[columna][index]}</td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="modal-footer">
              <button onClick={onClose} className="btn btn-secondary">
                Salir
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default ResultadosModal;

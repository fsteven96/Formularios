import React, { useState, useEffect } from "react";
import "./form-DeleteModal.css";
import axios from "axios";

function DeleteModal({ isOpen, onClose , id, nombreForm, fetchFormularios}) {
  const [error, setError] = useState(null);

  const handleDelete = async () => {
    try {
      console.log(id)
      await axios.delete(`http://localhost:5033/api/formulario/${id}`);
      
      onClose(); 
      fetchFormularios();
    } catch (err) {
      setError("Hubo un error al eliminar el formulario");
    }
  };

  if (!isOpen) return null;

  return (
    <>
      <div className="modal-backdrop-custom" onClick={onClose}></div>

      <div className="modal d-block" tabIndex="-1">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Agregar Formulario</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>

            <div className="modal-body">
              <div className="mb-3">
                <label htmlFor="nombreFormulario" className="form-label ">
                  Estas seguro que de desea elimnar formulario <span className="form-label fw-bold">{nombreForm}</span> 
                </label>
              </div>
            </div>

            <div className="modal-footer">
              <button onClick={onClose} className="btn btn-secondary" >
                Cancelar
              </button>
              <button onClick={handleDelete} className="btn btn-primary" >
                Aceptar
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default DeleteModal;

import React, { useState, useEffect } from "react";
import "./form-EditarOAgregarModal.css"; // Archivo para estilos personalizados
import axios from "axios";

function FormularioModal({ isOpen, onClose , fetchFormularios, formularioId }) {
  // const [formulario, setFormulario] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    Id: 0,
    nombre: "",
    campos: [ {Nombre: "", Tipo:"Texto", id:0}],
  });

  useEffect(() => {
    if (!isOpen) return;
    
    const fetchInput = async () => {
      try {
        console.log(formularioId)
        const response = await axios.get(`http://localhost:5033/api/formulario/${formularioId}`);
        // setFormulario(response.data);
        
        setFormData(response.data); 
        console.log(response.data)
        setLoading(false);
        setError(null);
      } catch (err) {
        setError("Hubo un error al cargar el formulario");
        setLoading(false);
      }
    };

    fetchInput();
  }, [isOpen, formularioId]);

  const handleInputChange = (e, index = null, field = null, i) => {
    console.log( index, field)
    const { name, value } = e.target;
  
    if (index !== null && field) {
      // Actualizar campo dinámico
      setFormData((prev) => {
        
        const nuevosCampos = [...prev.campos];
        nuevosCampos[i] = { ...nuevosCampos[i], [field]: value, ["id"]: index };
        return { ...prev, campos: nuevosCampos };
      });
    } else {
      // Actualizar nombre del formulario
      setFormData({ ...formData, [name]: value });
    }
    
  };

  const agregarCampo = () => {
    setFormData((prev) => ({
      ...prev,
      campos: [...prev.campos, { id: 0, Nombre: "", tipo: "texto" , FormularioId:0, "esNuevo": true}],
    }));
  };

  const eliminarCampo = (index) => {
    setFormData((prev) => ({
      ...prev,
      campos: prev.campos.filter((_, i) => i !== index),
    }));
  };

  const handleSave = async () => {
    try {
     
      if (!formData.nombre.trim()) {
        alert("El nombre del formulario no puede estar vacío.");
        return;
      }
  
      // Validar que todos los campos tengan un nombre
      for (let campo of formData.campos) {
        if (!campo.nombre.trim()) {
          alert("Todos los campos deben tener un nombre.");
          return;
        }
      }
      const formularioAEnviar = {
        Nombre: formData.nombre,
        Campos: formData.campos.map(campo => ({
          Nombre: campo.nombre,
          Tipo: campo.tipo
        }))
      };
      if (formularioAEnviar.Campos[0].Tipo == "") {
        formularioAEnviar.Campos[0].Tipo = "texto"
      }
      
      
      if (formularioId !== 0) {
        formularioAEnviar.Id = formularioId;
        formularioAEnviar.Campos = formularioAEnviar.Campos.map((campo, index) => {
          // Añadir el campo Id de cada campo
          console.log("Es nuevo ",formData.campos[index].esNuevo )
          if (formData.campos[index].esNuevo) {
            campo.Id = 0;
          } else {
            campo.Id = formData.campos[index].id;
          }
          
          return campo;
        });
        console.log("Respuesta del servidor:", formularioAEnviar);
        try {
          const response = await axios.put("http://localhost:5033/api/formulario", formularioAEnviar, {
            headers: {
              "Content-Type": "application/json",
            },
          });
          console.log("Formulario actualizado:", response.data);
        } catch (error) {
          console.error("Error al actualizar formulario:", error);
          alert("Hubo un error al actualizar el formulario.");
        }
      } else {
        console.log("Respuesta del servidor:", formularioAEnviar);
        try {
          const response = await axios.post("http://localhost:5033/api/formulario", formularioAEnviar, {
            headers: {
              "Content-Type": "application/json",
            },
          });
          console.log("Formulario guardado:", response);
        } catch (error) {
          console.error("Error al guardar formulario:", error);
          alert("Hubo un error al guardar el formulario.");
        }
      }
      
  
      
      alert("Formulario guardado correctamente.");
      onClose();
      fetchFormularios();
    } catch (error) {
      console.error("Error al guardar el formulario:", error);
      alert("Hubo un error al guardar el formulario.");
    }
  };
  

  if (!isOpen) return null;
  if (loading) return <div>Cargando...</div>;
  if (error) return <div>{error}</div>;

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
                <label htmlFor="nombreFormulario" className="form-label fw-bold">
                  Nombre del formulario
                </label>
                <input 
                  id="nombreFormulario"
                  type="text"
                  className="form-control"
                  value={formData.nombre || " "}
                  onChange={handleInputChange}
                  name="nombre"
                />
              </div>

              {formData.campos.map((campo, index) => (
                
                <div className="row mb-3 align-items-center" key={`temp-${index}`}>
                  <div className="col-5">
                    <label htmlFor={`nom${campo.id !== 0 ? campo.id : index}`} className="form-label">
                      Nombre
                    </label>
                    <input
                      id={`nom${campo.id !== 0 ? campo.id : index}`}
                      type="text"
                      className="form-control"
                      value={campo.nombre || ""}
                      onChange={(e) => handleInputChange(e, campo.id !== 0 ? campo.id : index, "nombre", index)}
                      name={`campo-${index}-nombre`}
                    />
                  </div>

                  <div className="col-5">
                    <label htmlFor={`tipo${campo.id !== 0 ? campo.id : index}`} className="form-label">
                      Tipo de datos
                    </label>
                    <select
                     id= {`tipo${campo.id !== 0 ? campo.id : index}`}
                      className="form-control"
                      value={campo.tipo || "texto"}
                      onChange={(e) => handleInputChange(e, campo.id !== 0 ? campo.id : index, "tipo", index)}
                      name={`campo-tipo-${campo.id !== 0 ? campo.id : index}tipo`}
                    >
                      <option value="texto">Texto</option>
                      <option value="numero">Número</option>
                      <option value="fecha">Fecha</option>
                      {/* <option value="booleano">Estado</option> */}
                    </select>
                  </div>

                  <div className="col-2 text-center">
                    <button className="btn btn-danger" onClick={() => eliminarCampo(index)}>
                      ❌
                    </button>
                  </div>
                </div>
              ))}

              <button className="btn btn-primary mt-2" onClick={agregarCampo}>
                ➕ Agregar columna
              </button>
            </div>

            <div className="modal-footer">
              <button className="btn btn-secondary" onClick={onClose}>
                Cerrar
              </button>
              <button className="btn btn-primary" onClick={handleSave}>
                Guardar
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default FormularioModal;


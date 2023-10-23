import React, { useState, useRef , useEffect } from "react";
import styles from "../../pages/UploadFiles/UploadFiles.module.css"; // Import module.css styles
import fileUploadStyles from "./FileUploadOptions.module.css"; // Import fileUploadOption.module.css
import readerService from "../../../services/readerService";
import MonthYearSelector from "../MonthYearSelector/MonthYearSelector";
import { Modal } from 'react-bootstrap';
import { useAuth } from "../../../context/AuthContext";

const DragDropFiles: React.FC = () => {
  const currentDate = new Date();
  const defaultMonth = currentDate.getMonth() + 1; // Months are zero-based, so add 1
  const defaultYear = currentDate.getFullYear();

  const [files, setFiles] = useState<FileList | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const [bankType, setBankType] = useState<string>(""); // Add bank type state
  const [selectedMonth, setSelectedMonth] = useState<number>(defaultMonth);
  const [selectedYear, setSelectedYear] = useState<number>(defaultYear);
  const [showSuccessModal, setShowSuccessModal] = useState(false); // State for success modal
  const { user } = useAuth();
  const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
  };

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    const droppedFiles = event.dataTransfer.files;
    if (droppedFiles.length > 1) {
      alert("Only one file is allowed at a time.");
    } else {
      setFiles(droppedFiles);
    }
  };

  const resetComponentState = () => {
    setFiles(null);
    setBankType("");
    setSelectedMonth(defaultMonth);
    setSelectedYear(defaultYear);
    setShowSuccessModal(false);
  };
  useEffect(() => {
    if (showSuccessModal) {
      const resetTimeout = setTimeout(() => {
        resetComponentState();
        setShowSuccessModal(false);
      }, 1500);
      
      return () => clearTimeout(resetTimeout);
    }
  }, [showSuccessModal]);
  const handleUpload = async (bankType:string, selectedMonth:number, selectedYear:number) => {
    if (files) {
      const file = files[0];
      try {
        const response = await readerService.createFilesPath(file, user.userID,bankType, selectedMonth, selectedYear);
        console.log('Upload successful:', response);
        setShowSuccessModal(true);
      } catch (error) {
        console.error('Upload failed:', error);
      }
    }
  };

  if (files) {
    return (
      <div className={`${styles.card} ${fileUploadStyles.uploads}`}>
        <h3 className={fileUploadStyles.uploadHeading}>Upload File</h3>
        <div className={`${styles.actions} ${fileUploadStyles.actions}`}>
        <ul>
          {Array.from(files).map((file, idx) => (
            <li key={idx}>{file.name}</li>
          ))}
        </ul>
        <div className={`${styles.row} ${fileUploadStyles.buttonsContainer}`}>
          <select className={`select ${fileUploadStyles.select}`} value={bankType} onChange={(event) => setBankType(event.target.value)}>
            <option value="">Bank Type</option>
            <option value="Habinleumi">Habinleumi</option>
            <option value="Hapoalim">Hapoalim</option>
            <option value="Max">Max</option>
          </select>
          <MonthYearSelector
              selectedMonth={selectedMonth}
              selectedYear={selectedYear}
              setSelectedMonth={setSelectedMonth}
              setSelectedYear={setSelectedYear}              
          />        
          <button className={`button ${fileUploadStyles.button}`} onClick={() => handleUpload(bankType, selectedMonth,selectedYear)}>Upload</button>         
          <button className={`button ${fileUploadStyles.button}`} onClick={() => setFiles(null)}>Cancel</button>
          {/* Modal for Success */}
          <Modal show={showSuccessModal} onHide={() => setShowSuccessModal(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Upload Successful!</Modal.Title>
          </Modal.Header>
          <Modal.Body>Your file has been uploaded successfully.</Modal.Body>                    
          </Modal>
        </div>
        </div>
      </div>
    );
  }

  return (
    <>
      <div
        className={`${styles.card} ${fileUploadStyles.dropzone}`}
        onDragOver={handleDragOver}
        onDrop={handleDrop}
      >
        <h3>Drag and Drop a File to Upload</h3>
        <h3>Or</h3>
        <input
          type="file"
          onChange={(event) => setFiles(event.target.files)}
          hidden
          accept=".xls, .xlsx, application/vnd.ms-excel"
          ref={inputRef}
        />
        <button onClick={() => inputRef.current?.click()}>Select File</button>
      </div>
    </>
  );
};

export default DragDropFiles;

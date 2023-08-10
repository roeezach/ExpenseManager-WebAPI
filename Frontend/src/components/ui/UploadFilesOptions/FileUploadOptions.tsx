import React, { useState, useRef } from "react";
import styles from "../../pages/UploadFiles/UploadFiles.module.css"; // Import module.css styles
import fileUploadStyles from "./FileUploadOptions.module.css"; // Import fileUploadOption.module.css
import readerService from "../../../services/readerService";

const DragDropFiles: React.FC = () => {
  const [files, setFiles] = useState<FileList | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);

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

  const handleUpload = async () => {
    if (files) {
      const file = files[0]; // Get the first file object
      const userId = 19773792; // Replace with the actual user ID once available
  
      try {
        const response = await readerService.createFilesPath(file, userId);
        console.log('Upload successful:', response);
      } catch (error) {
        console.error('Upload failed:', error);
      }
    }
  };

  if (files) {
    return (
      <div className={`${styles.card} ${styles.uploads}`}>
        <ul>
          {Array.from(files).map((file, idx) => (
            <li key={idx}>{file.name}</li>
          ))}
        </ul>
        <div className={`${styles.actions} ${fileUploadStyles.actions}`}>
          <button onClick={() => setFiles(null)}>Cancel</button>
          <button onClick={handleUpload}>Upload</button>
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
        <h1>Drag and Drop a File to Upload</h1>
        <h1>Or</h1>
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

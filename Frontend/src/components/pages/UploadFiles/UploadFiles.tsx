import React from 'react';
import styles from './UploadFiles.module.css'; // Import your CSS module

const UploadedFilesHistoryCard: React.FC = () => {
  // mocck data for uploaded files history
  const uploadedFiles = [
    { name: 'file1.xls', date: '2023-08-10' },
    { name: 'file2.xls', date: '2023-08-11' },
  ];

  return (
    <div className={`${styles.card}`}>
      <h2 className={`text-xl font-bold mb-4`}>Uploaded Files History</h2>
      <ul>
        {uploadedFiles.map((file, index) => (
          <li key={index} className={`mb-2`}>
            {file.name} - {file.date}
          </li>
        ))}
      </ul>
    </div>
  );
};

const FileUploadOptionsCard: React.FC = () => {
  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    // Handle file upload logic
  };

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    // Handle drop logic
  };

  return (
    <div className={`${styles.card}`}>
      <h2 className={`text-xl font-bold mb-4`}>File Upload Options</h2>
      <input
        type="file"
        accept=".xls"
        onChange={handleFileChange}
        className={`mb-4`}
      />
      <div
        className={`border-2 border-dashed p-4`}
        onDrop={handleDrop}
        onDragOver={(e) => e.preventDefault()}
      >
      </div>
    </div>
  );
};

const UploadFiles: React.FC = () => {
  return (
    <div className={`flex ${styles.centerContent} ${styles.fullHeight}`}>
      <div className={`${styles.container}`}>
        <UploadedFilesHistoryCard />
        <FileUploadOptionsCard />
      </div>
    </div>
  );
};

export default UploadFiles;

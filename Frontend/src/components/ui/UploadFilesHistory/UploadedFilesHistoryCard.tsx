import React, { useEffect, useState } from 'react';
import styles from '../../pages/UploadFiles/UploadFiles.module.css';
import readerService from '../../../services/readerService';
import historyStyles from './UploadedFilesHistoryCard.module.css';
import UploadedFileMapping from '../UploadedFileMapping/UploadedFileMapping';
import { Modal } from 'react-bootstrap';

const UploadedFilesHistoryCard = () => {
  const [uploadedFiles, setUploadedFiles] = useState([]);
  const [mappingModalOpen, setMappingModalOpen] = useState(false);

  useEffect(() => {
    const fetchUploadedFiles = async () => {
      try {
        const userId = parseInt(process.env.REACT_APP_USER_ID_TEMP, 10); // TODO - USER MANAGMENT
        const data = await readerService.getUploadedFiles(userId);
        setUploadedFiles(data);
      } catch (error) {
        console.error('Error fetching uploaded files:', error);
      }
    };
    fetchUploadedFiles();
  }, []);

  const formatUploadDate = (uploadDate: Date) => {
    const date = new Date(uploadDate);
    const formattedDate = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
    return formattedDate;
  };


  const handleMappingFinish = () => {
    setMappingModalOpen(false);
  };
  

  return (
    <div className={`${styles.card}`}>
      <div className="dropzone">
        <h2 className={`text-xl font-bold mb-4`}>Uploaded Files History</h2>
        <table className="uploaded-files-table">
          <thead>
            <tr>
              <th>File Name</th>
              <th>Type</th>
              <th>Month</th>
              <th>Year</th>
              <th>Upload Date</th>
            </tr>
          </thead>
          <tbody className={`${historyStyles.table_header}`}>
            {uploadedFiles.map((file, index) => (
              <tr key={index}>
                <td>{file.fileName}</td>
                <td>{file.fileType}</td>
                <td>{file.linkedMonth}</td>
                <td>{file.linkedYear}</td>
                <td>{formatUploadDate(file.uploadDate)}</td>
                <td>            
                  <UploadedFileMapping 
                  fileName={file.fileName} 
                  fileType={file.fileType} 
                  chargedDate={new Date(file.linkedYear, file.linkedMonth, 1)} //zero based
                  onMappingFinish={handleMappingFinish} />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {/* Modal for Success Message */}
      <Modal show={mappingModalOpen} onHide={handleMappingFinish}>
        <Modal.Header closeButton>
          <Modal.Title>Mapping Successful</Modal.Title>
        </Modal.Header>
        <Modal.Body>Your expenses have been mapped successfully.</Modal.Body>
      </Modal>
    </div>
  );
};

export default UploadedFilesHistoryCard;

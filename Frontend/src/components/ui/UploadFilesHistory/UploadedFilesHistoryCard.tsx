import React, { useEffect, useState } from 'react';
import styles from '../../pages/UploadFiles/UploadFiles.module.css';
import readerService from '../../../services/readerService';
import historyStyles from './UploadedFilesHistoryCard.module.css';
import UploadedFileMapping from '../UploadedFileMapping/UploadedFileMapping';
import { Modal } from 'react-bootstrap';
import { useAuth } from '../../../context/AuthContext';

const UploadedFilesHistoryCard: React.FC<{ shouldUpdate: boolean , setShouldUpdate : (flag: boolean) => void }> = ({ shouldUpdate , setShouldUpdate}) => {
  const [uploadedFiles, setUploadedFiles] = useState([]);
  
  const [mappingModalOpen, setMappingModalOpen] = useState(false);
  const { user } = useAuth();
  
  useEffect(() => {
    if (shouldUpdate && user) {
      console.log(`should update ${shouldUpdate}`);      
      fetchUploadedFiles();
      setShouldUpdate(false);
    }
    else
      fetchUploadedFiles();
  }, [shouldUpdate, user]);

  const fetchUploadedFiles = async () => {
    try {
      if(user)
      {
        const data = await readerService.getUploadedFiles(user.userID);
        setUploadedFiles(data);
      }
    } catch (error) {
      console.error('Error fetching uploaded files:', error);
    }
  };
  
  const formatUploadDate = (uploadDate: Date) => {
    const date = new Date(uploadDate);
    const formattedDate = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
    return formattedDate;
  };

  const handleMappingFinish = () => {
    setMappingModalOpen(false);
    fetchUploadedFiles();
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

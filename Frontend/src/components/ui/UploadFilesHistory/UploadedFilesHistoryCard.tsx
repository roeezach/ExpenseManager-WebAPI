import React, {useEffect, useState} from 'react';
import styles from '../../pages/UploadFiles/UploadFiles.module.css'; // Import your CSS module
import readerService from '../../../services/readerService';
import historyStyles from './UploadedFilesHistoryCard.module.css'

const UploadedFilesHistoryCard  = () => {
    const [uploadedFiles, setUploadedFiles] = useState([]);
  
    useEffect(() => {
      const fetchUploadedFiles = async () => {
        try {
          const userId = 19773792; // Replace with the actual user ID after logic functionality
          const data = await readerService.getUploadedFiles(userId);
          setUploadedFiles(data);
          console.log(uploadedFiles);
          
        } catch (error) {
          console.error('Error fetching uploaded files:', error);
        }
      }; 
      fetchUploadedFiles();
    }, []);
   
    const formatUploadDate = (uploadDate : Date) => {
      const date = new Date(uploadDate);
      const formattedDate = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
      return formattedDate;
    };

    return (
      <div className={`${styles.card}`}>
        <div className="dropzone">
          <h2 className={`text-xl font-bold mb-4`}>Uploaded Files History</h2>
          <table className="uploaded-files-table">
            <thead>
              <tr>
                <th>File Name</th>
                <th>Upload Date</th>
              </tr>
            </thead>
            <tbody className={`${historyStyles.table_header}`}>
              {uploadedFiles.map((file, index) => (
                <tr key={index}>
                  <td>{file.fileName}</td>
                  <td>{formatUploadDate(file.uploadDate)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  };
  
export default UploadedFilesHistoryCard;

import React from 'react';
import styles from './UploadFiles.module.css';
import FileUploadOptions from '../../ui/UploadFilesOptions/FileUploadOptions';
import UploadedFilesHistoryCard from '../../ui/UploadFilesHistory/UploadedFilesHistoryCard';

const UploadFiles: React.FC = () => {
  return (
    <div className={`flex ${styles.cardGrid}`}>
      <div className={`flex ${styles.centerContent} ${styles.fullHeight}`}>
        <div className={`${styles.container}`}>
          <UploadedFilesHistoryCard />
        </div>        
      </div>  
      
      <div className={`flex ${styles.centerContent} ${styles.fullHeight}`}>
        <div className={`${styles.container}`}>
        <FileUploadOptions/>
        </div>        
      </div>  
      </div>
 );
};

export default UploadFiles;

import React, { useState } from 'react';
import styles from './UploadFiles.module.css';
import FileUploadOptions from '../../ui/UploadFilesOptions/FileUploadOptions';
import UploadedFilesHistoryCard from '../../ui/UploadFilesHistory/UploadedFilesHistoryCard';

const UploadFiles: React.FC = () => {

  const [shouldUpdateHistory, setShouldUpdateHistory] = useState(false);

  const updateHistory = (value : boolean) => {
     setShouldUpdateHistory(value);
  };
  
  return (
    <div className={`flex ${styles.cardGrid}`}>
      <div className={`flex ${styles.centerContent} ${styles.fullHeight}`}>
        <div className={`${styles.container}`}>
        <UploadedFilesHistoryCard shouldUpdate={shouldUpdateHistory} setShouldUpdate={setShouldUpdateHistory} />
        </div>        
      </div>  
      
      <div className={`flex ${styles.centerContent} ${styles.fullHeight}`}>
        <div className={`${styles.container}`}>
          <FileUploadOptions onUploadSuccess={updateHistory} />
        </div>        
      </div>  
      </div>
 );
};

export default UploadFiles;

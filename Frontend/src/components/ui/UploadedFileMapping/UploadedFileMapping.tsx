import React, { useState } from 'react';
import mapperService from '../../../services/mapperService';
import swExpensesService from '../../../services/splitewiseExpenseService';
import recalculateExpenseService from '../../../services/recalculateExpenseService';
import totalExpensePerCategoryService from '../../../services/totalExpensePerCategoryService';
import { Button, Modal } from 'react-bootstrap';
import { monthDayYearDateFormat } from '../../../utils/DateUtils'
import { useAuth } from '../../../context/AuthContext';

interface Props {
    fileName: string;
    fileType: string;
    chargedDate: Date;
    onMappingFinish: () => void;
  }

const UploadedFileMapping: React.FC<Props> = ({ fileName, fileType, chargedDate,onMappingFinish }) => {
    const [loading, setLoading] = useState(false);
    const [mappingModalOpen, setMappingModalOpen] = useState(false);
    const [errorModalOpen, setErrorModalOpen] = useState(false); 
    const { user } = useAuth();
  const handleButtonClick = async () => {
    console.log('clicked happened');
    
    try {
    setLoading(true);
    await HanleFileExpenses(chargedDate, fileName, fileType);

    const parsedDate = FixChargeDateToFromDate(chargedDate);
       
    await HandleSplitwiseExpenses(parsedDate);
            
    await HandleRecalculation(parsedDate);
        
    await HandleTotalsPerCategory(parsedDate);
     
    setLoading(false);
    setMappingModalOpen(true); 
    onMappingFinish();

    } catch (error) {
      console.error('Error in upload details:', error);
      setLoading(false);
      setErrorModalOpen(true);
    }
  };

  function FixChargeDateToFromDate(chargedDate: Date) {
    const parsedDate = new Date(chargedDate);
    console.log(`date before fixing ${chargedDate}`);
    
    parsedDate.setMonth(parsedDate.getMonth() - 1);

    if (chargedDate.getMonth() === 0) {
        parsedDate.setFullYear(parsedDate.getFullYear() - 1);
        parsedDate.setMonth(11);
    }
    console.log(`date after fixing ${parsedDate}`);
    return parsedDate;
}

const formatDate = (date:Date) => {
  const day = date.getDate().toString().padStart(2, '0');
  const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
  const year = date.getFullYear().toString();

  return `${month}/${day}/${year}`;
};

async function HanleFileExpenses(chargedDate: Date, fileName: string, fileType: string) {
    let isExpensesCreated = false;
    console.log(`the date is ${chargedDate}`);
    
    const monthlyExpensesResponse = await mapperService.getMonthlyExpenses(chargedDate);
    if (monthlyExpensesResponse.length === 0) {
        const formattedChargedDate = formatDate(chargedDate);
        await mapperService.createMappedExpenses(fileName, fileType, formattedChargedDate);
        isExpensesCreated = true;
    }

    // Check for SW user ID and call SW expenses service if needed - TODO
    if (!isExpensesCreated) {
        //return model saying the data already exist 
        //implenet sw logic
    }
}

async function HandleRecalculation(parsedDate: Date) {
  const formattedFromDate = monthDayYearDateFormat(parsedDate)
  console.log(`the date is ${formattedFromDate}`);
    const rcGetResponse = await recalculateExpenseService.getRecalculateExpenses(formattedFromDate);
    if (rcGetResponse.length === 0)
        await recalculateExpenseService.createRecalculateExpenses(formattedFromDate);
}

async function HandleSplitwiseExpenses(parsedDate: Date) {
    const formattedFormDate = monthDayYearDateFormat(parsedDate)
    const swGetResponse = await swExpensesService.getSplitwiseExpenses(formattedFormDate);
    if (swGetResponse.length === 0)
        await swExpensesService.createSplitwiseExpenses(formattedFormDate);
}

async function HandleTotalsPerCategory(parsedDate: Date) {
    const formattedFormDate = monthDayYearDateFormat(parsedDate)
    const resGetTotalCategoriesSum = await totalExpensePerCategoryService.getCategoriesSumPerTimePeriod(parsedDate.getMonth() + 1, parsedDate.getFullYear(), user.userID);
    if (JSON.stringify(resGetTotalCategoriesSum) === '{}')
        await totalExpensePerCategoryService.CreateCategoriesSum(formattedFormDate, user.userID);
}

//TODO - make this component genric after implemenation of user managment
return (
    <div>
      <Button onClick={handleButtonClick} disabled={loading}   style={{ background: 'white', color:'black' }}>
        {loading ? 'Mapping...' : 'Map'}
      </Button>
      {/* Modal for Mapping Success */}
      <Modal show={mappingModalOpen} onHide={() => setMappingModalOpen(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Successful Mapping!</Modal.Title>
        </Modal.Header>
        <Modal.Body>Your expenses have been mapped successfully.</Modal.Body>
      </Modal>
      {/* Modal for Error */}
      <Modal show={errorModalOpen} onHide={() => setErrorModalOpen(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Something Went Wrong</Modal.Title>
        </Modal.Header>
      <Modal.Body>Consider Contact support for assistance.</Modal.Body>
    </Modal>
    </div>
  );
};

export default UploadedFileMapping;
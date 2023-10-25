import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import BarGraph from '../../ui/Graph/BarGraph/BarGraph';
import PieChartComponent from '../../ui/Graph/PieGraph/PieGraph';
import styles from './Overview.module.css';
import totalExpensePerCategoryService from '../../../services/totalExpensePerCategoryService';
import MonthYearSelector from '../../ui/MonthYearSelector/MonthYearSelector';
import { useAuth } from '../../../context/AuthContext';

const Homepage: React.FC = () => {
  const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
  const [selectedYear, setSelectedYear] = useState(new Date().getFullYear());
  const [monthData, setMonthData] = useState([]);
  const { user } = useAuth();
  useEffect(() => {
    fetchMonthData(selectedMonth, selectedYear);
  }, [selectedMonth, selectedYear]);

  const fetchMonthData = async (month: number, year: number) => {
    try {      
      console.log(`the month is ${month}`);
      const data = await totalExpensePerCategoryService.getCategoriesSumPerTimePeriod(month, year, user.userID);

      if (data.length === 0) {
        console.log('No data available.');
      } else {
        console.log('Data received:', data);
        setMonthData(data);
      }
    } catch (error) {
      console.error('Error fetching month data:', error);
    }
  };

  const dataArray = Object.entries(monthData).map(([name, value]) => ({ name, value }));
  const totalValue = dataArray.reduce((sum, entry) => sum + entry.value, 0);

  return (
    <div className={styles.card}>
      <div className={styles.centerContent}>
        <div className={styles.adjustRight}>
          <h2>Overview Page</h2>
          <div className={`${styles.row} ${styles.centerContainer}`}>
             <MonthYearSelector
              selectedMonth={selectedMonth}
              selectedYear={selectedYear}
              setSelectedMonth={setSelectedMonth}
              setSelectedYear={setSelectedYear}              
            />              
          </div>
          {Object.keys(monthData).length === 0 ? (
            <p>
              No data available for the selected month. <NavLink to="/files">Upload data</NavLink>
            </p>
          ) : (
            <div className={styles.cardContent}>
              <div className={styles.graphContainer}>
                <div className={styles.chartContainer}>
                  <BarGraph data={monthData} title="Monthly Expenses" />
                </div>
                <div className={styles.chartContainer}>
                  <PieChartComponent data={monthData} title="Expenses Chart" />
                </div>
              </div>
                <h3>Expenses Amounts</h3>
              <div className={styles.tableContainer}>
                <table className={styles.table}>
                  <thead>
                    <tr>
                      <th>Category</th>
                      <th>Value</th>
                    </tr>
                  </thead>
                  <tbody>
                    {Object.entries(monthData).map(([name, value], index) => (
                      <tr key={index}>
                        <td>{name}</td>
                        <td>{value.toFixed(2)} ₪</td>
                      </tr>
                    ))}
                    <tr>
                      <td><strong>Total Expenses Amount</strong></td>
                      <td>{totalValue.toFixed(2)}₪</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Homepage;

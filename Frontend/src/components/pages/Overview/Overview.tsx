import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import BarGraph from '../../ui/Graph/BarGraph/BarGraph';
import PieChartComponent from '../../ui/Graph/PieGraph/PieGraph';
import styles from './Overview.module.css';
import totalExpensePerCategoryService from '../../../services/totalExpensePerCategoryService';

const Homepage: React.FC = () => {
  const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
  const [selectedYear, setSelectedYear] = useState(new Date().getFullYear());
  const [monthData, setMonthData] = useState([]);

  useEffect(() => {
    fetchMonthData(selectedMonth + 1, selectedYear);
  }, [selectedMonth, selectedYear]);

  const fetchMonthData = async (month: number, year: number) => {
    try {
      const currUserId = 19773792;
      const data = await totalExpensePerCategoryService.getCategoriesSumPerTimePeriod(month, year, currUserId);

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

  const years = Array.from({ length: new Date().getFullYear() - 2021 + 1 }, (_, index) => 2021 + index);
  const months = Array.from({ length: 12 }, (_, index) => index + 1);
  const dataArray = Object.entries(monthData).map(([name, value]) => ({ name, value }));
  const totalValue = dataArray.reduce((sum, entry) => sum + entry.value, 0);

  return (
    <div className={styles.card}>
      <div className={styles.centerContent}>
        <div className={styles.adjustRight}>
          <h2>Overview Page</h2>
          <div className={`${styles.row} ${styles.centerContainer}`}>
            <select className={styles.select} value={selectedMonth} onChange={e => setSelectedMonth(Number(e.target.value))}>
              {months.map(month => (
                <option key={month} value={month}>
                  {new Date(0, month - 1).toLocaleString('default', { month: 'long' })}
                </option>
              ))}
            </select>
            <select className={styles.select} value={selectedYear} onChange={e => setSelectedYear(Number(e.target.value))}>
              {years.map(year => (
                <option key={year} value={year}>
                  {year}
                </option>
              ))}
            </select>
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
                      <td>{totalValue}</td>
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

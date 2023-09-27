import React from 'react';

interface MonthYearInputProps {
    selectedMonth: number;
    selectedYear: number;
    setSelectedMonth: (month: number) => void;
    setSelectedYear: (year: number) => void;
  }

const MonthYearSelector : React.FC<MonthYearInputProps>= ({ selectedMonth, selectedYear, setSelectedMonth, setSelectedYear }) => {
  const years = Array.from({ length: new Date().getFullYear() - 2021 + 1 }, (_, index) => 2021 + index);
  const months = Array.from({ length: 12 }, (_, index) => index + 1);

  return (
    <div>
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
  );
};

export default MonthYearSelector;

import React from 'react';
import { PieChart, Pie, Cell, Tooltip } from 'recharts';

interface PieChartProps {
  data: any[];
  title: string;
}

const generateColors = (count: number) => {
  const colors = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#FF9966', '#FF6600', '#CC3333', '#663300', '#660099', '#003399'];
  const generatedColors: string[] = [];

  for (let i = 0; i < count; i++) {
    generatedColors.push(colors[i % colors.length]);
  }

  return generatedColors;
};

const PieChartComponent: React.FC<PieChartProps> = ({ data, title }) => {
  const dataArray = Object.entries(data).map(([name, value]) => ({ name, value }));
  const totalValue = dataArray.reduce((sum, entry) => sum + entry.value, 0);
  const sortedDataArray = dataArray.sort((a, b) => b.value - a.value); // Sort data by value in descending order
  const colors = generateColors(dataArray.length);

  const filteredDataArray = sortedDataArray.filter(entry => (entry.value / totalValue) * 100 >= 2);

  return (
    <div>
      <h3>{title}</h3>
      <PieChart width={1000} height={300}>
        <Pie
          dataKey="value"
          isAnimationActive={true}
          data={filteredDataArray}
          cx="50%"
          cy="50%"
          outerRadius={125}
          fill="#8884d8"
          label={entry => `${entry.name}: ${entry.value}₪ (${((entry.value / totalValue) * 100).toFixed(2)}%)`}
        >
          {filteredDataArray.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={colors[index]} />
          ))}
        </Pie>
        <Tooltip formatter={(value, name, entry) => [value, `(${((entry.payload.value / totalValue) * 100).toFixed(2)}%)`]} />
      </PieChart>
    </div>
  );
};

export default PieChartComponent;

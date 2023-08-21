import React from 'react';
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip } from 'recharts';
import styles from './BarGraph.module.css';

interface GraphProps {
  data: unknown[];
  title: string;
  children?: React.ReactNode;
}

const BarGraph: React.FC<GraphProps> = ({ data, title }) => {
  return (
    <div className={styles.rechartsWrapper}>
      <h3>{title}</h3>
      <ResponsiveContainer className={styles.centerContainer} width={500} height={400}>
        <BarChart
          width={800}
          height={400}
          data={Object.entries(data).map(([category, expenses]) => ({ category, expenses }))}
          margin={{ top: 0, right: 20, left: 30, bottom: 100 }}
        >
          <XAxis
            dataKey="category"
            type="category"
            interval={0}
            angle={-35}
            textAnchor="end"
            tick={<CustomizedAxisTick />}
          />
          <YAxis />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip />
          <Bar dataKey="expenses" fill="#8fb7a1" />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};

const CustomizedAxisTick: React.FC<any> = (props) => {
  const { x, y, payload } = props;
  return (
    <g transform={`translate(${x},${y})`}>
      <text
        x={0}
        y={0}
        dy={16}
        textAnchor="end"
        fill="#666"
        transform="rotate(-25)"
        fontSize={10}
        fontWeight="bold"
      >
        {payload.value}
      </text>
    </g>
  );
};

export default BarGraph;

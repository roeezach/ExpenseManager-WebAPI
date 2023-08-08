import React, { useState, useEffect } from 'react';
import styles from './EditCategory.module.css'; // Import your CSS module

interface Category {
  id: number;
  name: string;
}

const EditCategories: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    // Fetch categories from backend and update state
  }, []);

  const handleDeleteCategory = (id: number) => {
    // Handle category deletion
  };

  return (
    <div className={`${styles.editCategoriesContainer}`}>
      <h2 className={`text-2xl font-bold mb-4`}>Edit Categories</h2>
      <ul className={`${styles.categoryList}`}>
        {categories.map((category) => (
          <li key={category.id} className={`${styles.categoryItem}`}>
            <span className={`${styles.categoryName}`}>{category.name}</span>
            <button
              onClick={() => handleDeleteCategory(category.id)}
              className={`${styles.deleteButton}`}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EditCategories;
